using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using NTranslate.Dto;

namespace NTranslate
{
    public class Project : IDisposable
    {
        private static readonly string[] IncludedExtensions = { ".resx" };
        private readonly string _translationsPath;
        private readonly TranslationCollection _translations = new TranslationCollection();
        private bool _disposed;

        public static Project FindProject(ProjectItem projectItem)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            while (projectItem != null)
            {
                var project = projectItem.GetProperty<Project>();
                if (project != null)
                    return project;

                projectItem = projectItem.Parent;
            }

            throw new InvalidOperationException("Cannot find project");
        }

        public ProjectItem RootNode { get; private set; }
        public string Directory { get; private set; }
        public string Namespace { get; private set; }

        public Project(Solution solution, string fileName, string name)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (name == null)
                throw new ArgumentNullException("name");

            RootNode = new ProjectItem(fileName, name, false);
            RootNode.SetProperty<Project>(this);

            Namespace = GetNamespace(fileName);

            Directory = Path.GetDirectoryName(RootNode.FileName);

            _translationsPath = Path.Combine(Directory, "Translations");

            LoadTranslations();
            LoadProject(RootNode, new DirectoryInfo(Path.GetDirectoryName(fileName)), "");

            UpdateFromLanguage(solution.Dictionary);

            Program.MainForm.LanguageChanged += MainForm_LanguageChanged;
        }

        private string GetNamespace(string fileName)
        {
            var document = XDocument.Load(fileName);
            var ns = (XNamespace)"http://schemas.microsoft.com/developer/msbuild/2003";

            foreach (var element in document.Root.Elements(ns + "PropertyGroup"))
            {
                foreach (var namespaceElement in element.Elements(ns + "RootNamespace"))
                {
                    return namespaceElement.Value;
                }
            }

            throw new InvalidOperationException("Expected a root namespace");
        }

        void MainForm_LanguageChanged(object sender, EventArgs e)
        {
            UpdateFromLanguage(Program.SolutionManager.CurrentSolution.Dictionary);
        }

        private void LoadTranslations()
        {
            if (System.IO.Directory.Exists(_translationsPath))
            {
                foreach (string fileName in System.IO.Directory.GetFiles(_translationsPath, "*" + TranslationFile.Extension))
                {
                    _translations.Add(new TranslationFile(fileName, Namespace));
                }
            }
        }

        private bool LoadProject(ProjectItem node, DirectoryInfo directory, string path)
        {
            var fileSystemInfos = new List<FileSystemInfo>(directory.GetFileSystemInfos().Where(Include));

            fileSystemInfos.Sort((a, b) =>
            {
                bool aDirectory = a is DirectoryInfo;
                bool bDirectory = b is DirectoryInfo;

                if (aDirectory != bDirectory)
                    return bDirectory ? 1 : -1;

                return String.Compare(a.Name, b.Name, StringComparison.CurrentCultureIgnoreCase);
            });

            bool hadAny = false;

            foreach (var entry in fileSystemInfos)
            {
                var entryNode = new ProjectItem(path + entry.Name, entry is DirectoryInfo);

                if (entryNode.IsDirectory && !LoadProject(entryNode, (DirectoryInfo)entry, path + entry.Name + Path.DirectorySeparatorChar))
                    continue;

                hadAny = true;

                node.Children.Add(entryNode);
            }

            return hadAny;
        }

        private void UpdateFromLanguage(TranslationDictionary dictionary)
        {
            var language = Program.MainForm.Language;
            TranslationFile file = null;
            if (language != null && _translations.Contains(language))
                file = _translations[language];

            UpdateFromLanguage(
                dictionary,
                RootNode,
                file
            );
        }

        private void UpdateFromLanguage(TranslationDictionary dictionary, ProjectItem projectItem, TranslationFile file)
        {
            if (String.Equals(".resx", Path.GetExtension(projectItem.FileName), StringComparison.OrdinalIgnoreCase))
                UpdateProjectItemState(dictionary, projectItem, file);

            foreach (var child in projectItem.Children)
            {
                UpdateFromLanguage(dictionary, child, file);
            }
        }

        public void UpdateProjectItemState(ProjectItem projectItem, TranslationFile file)
        {
            UpdateProjectItemState(Program.SolutionManager.CurrentSolution.Dictionary, projectItem, file);
        }

        private void UpdateProjectItemState(TranslationDictionary dictionary, ProjectItem projectItem, TranslationFile file)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            var fileContents = FileContents.Load(
                this,
                projectItem,
                file != null ? file.FindFile(projectItem) : null
            );

            bool anyPending = false;

            foreach (var node in fileContents.Nodes)
            {
                if (
                    node.Source != node.OriginalSource ||
                    (!node.Hidden && node.Translated == null)
                ) {
                    anyPending = true;
                }

                if (node.Translated != null)
                    dictionary.Add(node.OriginalSource, node.Translated);
            }

            if (fileContents.Nodes.Count == 0)
                projectItem.State = ProjectItemState.Unknown;
            else
                projectItem.State = anyPending ? ProjectItemState.Incomplete : ProjectItemState.Complete;
        }

        private bool Include(FileSystemInfo entry)
        {
            return 
                entry is DirectoryInfo ||
                Array.IndexOf(IncludedExtensions, entry.Extension.ToLowerInvariant()) != -1;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Program.MainForm.LanguageChanged -= MainForm_LanguageChanged;

                _disposed = true;
            }
        }

        public void SaveTranslations(CultureInfo cultureInfo, ProjectItem projectItem, List<NodeDto> nodes)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");
            if (nodes == null)
                throw new ArgumentNullException("nodes");

            GetTranslations(cultureInfo).Save(projectItem, nodes);
        }

        private TranslationFile GetTranslations(CultureInfo cultureInfo)
        {
            if (!_translations.Contains(cultureInfo))
            {
                string fileName = Path.Combine(
                    _translationsPath,
                    cultureInfo.Name + TranslationFile.Extension
                );

                System.IO.Directory.CreateDirectory(_translationsPath);

                _translations.Add(new TranslationFile(fileName, Namespace));
            }

            return _translations[cultureInfo];
        }

        public FileDto LoadTranslations(CultureInfo cultureInfo, ProjectItem projectItem)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            if (_translations.Contains(cultureInfo))
                return _translations[cultureInfo].FindFile(projectItem);

            return null;
        }

        public IEnumerable<CultureInfo> GetTranslatedLanguages()
        {
            return _translations.Select(p => p.Language);
        }

        public void AddLanguage(CultureInfo cultureInfo)
        {
            if (cultureInfo == null)
                throw new ArgumentNullException("cultureInfo");

            GetTranslations(cultureInfo);
        }
    }
}
