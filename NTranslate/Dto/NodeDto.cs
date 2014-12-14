using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NTranslate.Dto
{
    [XmlRoot("Node", Namespace = TranslationsDto.Ns)]
    public class NodeDto
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }

        public string Source { get; set; }

        public string Text { get; set; }
    }
}
