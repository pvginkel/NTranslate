using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NTranslate
{
    public class Project : IDisposable
    {
        private static readonly string[] IncludedExtensions = { ".resx" };
        private readonly string _translationsPath;
        private bool _disposed;

        public ProjectItem RootNode { get; private set; }
        public string Directory { get; private set; }
        public TranslationCollection Translations { get; private set; }
        public string Namespace { get; private set; }

        public Project(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            RootNode = new ProjectItem(fileName, false);
            Translations = new TranslationCollection();
            Namespace = GetNamespace(fileName);

            Directory = Path.GetDirectoryName(RootNode.FileName);

            _translationsPath = Path.Combine(Directory, "Translations");

            LoadTranslations();
            LoadProject(RootNode, new DirectoryInfo(Path.GetDirectoryName(fileName)), "");

            UpdateFromLanguage();

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
            UpdateFromLanguage();
        }

        private void LoadTranslations()
        {
            if (System.IO.Directory.Exists(_translationsPath))
            {
                foreach (string fileName in System.IO.Directory.GetFiles(_translationsPath, "*" + TranslationFile.Extension))
                {
                    Translations.Add(new TranslationFile(fileName, Namespace));
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

        private void UpdateFromLanguage()
        {
            var language = Program.MainForm.Language;
            TranslationFile file = null;
            if (language != null && Translations.Contains(language))
                file = Translations[language];

            UpdateFromLanguage(RootNode, file);
        }

        private void UpdateFromLanguage(ProjectItem projectItem, TranslationFile file)
        {
            if (String.Equals(".resx", Path.GetExtension(projectItem.FileName), StringComparison.OrdinalIgnoreCase))
                UpdateProjectItemState(projectItem, file);

            foreach (var child in projectItem.Children)
            {
                UpdateFromLanguage(child, file);
            }
        }

        public void UpdateProjectItemState(ProjectItem projectItem, TranslationFile file)
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
                    break;
                }
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

        public void AddLanguage(CultureInfo cultureInfo)
        {
            string fileName = Path.Combine(
                _translationsPath,
                cultureInfo.Name + TranslationFile.Extension
            );

            System.IO.Directory.CreateDirectory(_translationsPath);

            Translations.Add(new TranslationFile(fileName, Namespace));
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                Program.MainForm.LanguageChanged -= MainForm_LanguageChanged;

                _disposed = true;
            }
        }
    }
}
