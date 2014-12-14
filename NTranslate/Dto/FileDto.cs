using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NTranslate.Dto
{
    [XmlRoot("File", Namespace = TranslationsDto.Ns)]
    public class FileDto
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        [XmlElement("Node", Namespace = TranslationsDto.Ns)]
        public List<NodeDto> Nodes { get; set; }

        public FileDto()
        {
            Nodes = new List<NodeDto>();
        }
    }
}
