using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using NTranslate.App.Dto;

namespace NTranslate.App
{
    public class TranslationFile
    {
        private static readonly XmlSerializer Serializer = new XmlSerializer(typeof(TranslationsDto));

        public const string Extension = "*.ntx";

        private readonly TranslationsDto _translations;

        public string FileName { get; private set; }

        public string Language
        {
            get { return _translations.Language; }
        }

        public TranslationFile(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileName = fileName;


            if (File.Exists(fileName))
            {
                using (var stream = File.OpenRead(fileName))
                {
                    _translations = (TranslationsDto)Serializer.Deserialize(stream);
                }
            }
            else
            {
                _translations = new TranslationsDto
                {
                    Language = Path.GetFileNameWithoutExtension(fileName)
                };
            }
        }

        public void Save(FileDto file)
        {
            if (file == null)
                throw new ArgumentNullException("file");

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
            {
                Serializer.Serialize(stream, _translations);
            }
        }

        private void Sort()
        {
            _translations.Files.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));

            foreach (var file in _translations.Files)
            {
                file.Nodes.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}
