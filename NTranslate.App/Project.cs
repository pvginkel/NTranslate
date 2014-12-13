using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NTranslate.App.Dto;

namespace NTranslate.App
{
    public class Project
    {
        private static readonly string[] IncludedExtensions = { ".resx" };

        public ProjectItem RootNode { get; private set; }
        public TranslationCollection Translations { get; private set; }

        public Project(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            RootNode = new ProjectItem(fileName, false);
            Translations = new TranslationCollection();

            LoadTranslations();
            LoadProject(RootNode, new DirectoryInfo(Path.GetDirectoryName(fileName)));
        }

        private void LoadTranslations()
        {
            string path = Path.Combine(
                Path.GetDirectoryName(RootNode.FileName),
                "Translations"
            );

            if (Directory.Exists(path))
            {
                foreach (string fileName in Directory.GetFiles(path, TranslationFile.Extension))
                {
                    Translations.Add(new TranslationFile(fileName));
                }
            }
        }

        private bool LoadProject(ProjectItem node, DirectoryInfo directory)
        {
            var fileSystemInfos = new List<FileSystemInfo>(directory.GetFileSystemInfos().Where(p => Include(p)));

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
                var entryNode = new ProjectItem(entry.FullName, entry is DirectoryInfo);

                if (entryNode.IsDirectory && !LoadProject(entryNode, (DirectoryInfo)entry))
                    continue;

                hadAny = true;

                node.Children.Add(entryNode);
            }

            return hadAny;
        }

        private bool Include(FileSystemInfo entry)
        {
            return 
                entry is DirectoryInfo ||
                Array.IndexOf(IncludedExtensions, entry.Extension.ToLowerInvariant()) != -1;
        }
    }
}
