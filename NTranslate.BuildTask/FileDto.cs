using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace NTranslate.BuildTask
{
    internal class FileDto
    {
        private const string Ns = "https://github.com/pvginkel/NTranslate/Translations";

        public static IList<FileDto> Load(XmlReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            if (!reader.ReadToFollowing("Translations", Ns))
                return null;

            var files = new List<FileDto>();

            while (true)
            {
                if (!reader.Read())
                    return null;
                if (reader.NodeType == XmlNodeType.Whitespace)
                    continue;
                if (reader.NodeType == XmlNodeType.EndElement)
                    break;
                if (reader.NodeType != XmlNodeType.Element)
                    return null;

                var file = ReadFile(reader);
                if (file == null)
                    return null;

                files.Add(file);
            }

            return files;
        }

        private static FileDto ReadFile(XmlReader reader)
        {
            if (reader.Name != "File" || reader.NamespaceURI != Ns)
                return null;

            string name = reader.GetAttribute("name");
            if (name == null)
                return null;

            var file = new FileDto(name);

            while (true)
            {
                if (!reader.Read())
                    return null;
                if (reader.NodeType == XmlNodeType.Whitespace)
                    continue;
                if (reader.NodeType == XmlNodeType.EndElement)
                    break;
                if (reader.NodeType != XmlNodeType.Element)
                    return null;

                var node = ReadNode(reader);
                if (node == null)
                    return null;

                file.Nodes.Add(node);
            }

            return file;
        }

        private static NodeDto ReadNode(XmlReader reader)
        {
            if (reader.Name != "Node" || reader.NamespaceURI != Ns)
                return null;

            string name = reader.GetAttribute("name");
            string text = null;

            while (true)
            {
                if (!reader.Read())
                    return null;
                if (reader.NodeType == XmlNodeType.EndElement)
                    break;
                if (reader.NodeType != XmlNodeType.Element)
                    continue;
                if (reader.NamespaceURI != Ns)
                    return null;

                switch (reader.Name)
                {
                    case "Source":
                        break;

                    case "Text":
                        if (!reader.Read() || reader.NodeType != XmlNodeType.Text)
                            return null;
                        text = reader.ReadContentAsString();
                        break;

                    default:
                        return null;
                }

                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    reader.Read();
                }
            }

            reader.ReadEndElement();

            if (name == null || text == null)
                return null;

            return new NodeDto(name, text);
        }

        public string Name { get; private set; }
        public IList<NodeDto> Nodes { get; private set; }

        public FileDto(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            Nodes = new List<NodeDto>();
        }
    }
}
