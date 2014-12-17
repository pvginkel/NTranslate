using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NTranslate.Dto;

namespace NTranslate
{
    public class TranslationFile
    {
        private readonly string _namespace;
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(TranslationsDto));
        private static readonly UTF8Encoding Encoding = new UTF8Encoding(true);

        public const string Extension = ".ntx";

        private readonly TranslationsDto _translations;

        public string FileName { get; private set; }

        public CultureInfo Language { get; private set; }

        public TranslationFile(string fileName, string @namespace)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (@namespace == null)
                throw new ArgumentNullException("namespace");

            FileName = fileName;
            _namespace = @namespace;

            if (File.Exists(fileName))
            {
                using (var stream = File.OpenRead(fileName))
                using (var reader = new StreamReader(stream, Encoding))
                {
                    _translations = (TranslationsDto)Serializer.Deserialize(reader);
                }
            }
            else
            {
                _translations = new TranslationsDto();
            }

            Language = CultureInfo.GetCultureInfo(Path.GetFileNameWithoutExtension(fileName));
        }

        public void Save(ProjectItem projectItem, List<NodeDto> nodes)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");
            if (nodes == null)
                throw new ArgumentNullException("nodes");

            nodes.RemoveAll(p => String.IsNullOrEmpty(p.Text));

            var file = new FileDto
            {
                Name = GetFileName(projectItem),
                Nodes = nodes
            };

            int index = _translations.Files.FindIndex(p => p.Name == file.Name);
            if (index == -1)
            {
                if (file.Nodes.Count > 0)
                    _translations.Files.Add(file);
            }
            else
            {
                if (file.Nodes.Count == 0)
                    _translations.Files.RemoveAt(index);
                else
                    _translations.Files[index] = file;
            }

            Sort();

            using (var stream = File.Create(FileName))
            using (var writer = new StreamWriter(stream, Encoding))
            {
                Serializer.Serialize(writer, _translations);
            }

            Project.FindProject(projectItem).UpdateProjectItemState(projectItem, this);
        }

        private void Sort()
        {
            _translations.Files.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

            foreach (var file in _translations.Files)
            {
                file.Nodes.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            }
        }

        public FileDto FindFile(ProjectItem projectItem)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            var name = GetFileName(projectItem);

            return _translations.Files.SingleOrDefault(p => p.Name == name);
        }

        private string GetFileName(ProjectItem projectItem)
        {
            return
                _namespace + "." +
                projectItem.FileName.Replace(Path.DirectorySeparatorChar, '.');
        }
    }
}
