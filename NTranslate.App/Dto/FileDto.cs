using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NTranslate.App.Dto
{
    [XmlType("File", Namespace = TranslationsDto.Ns)]
    public class FileDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        public List<NodeDto> Nodes { get; set; }

        public FileDto()
        {
            Nodes = new List<NodeDto>();
        }
    }
}
