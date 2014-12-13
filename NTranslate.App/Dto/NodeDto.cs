using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NTranslate.App.Dto
{
    [XmlType("Node", Namespace = TranslationsDto.Ns)]
    public class NodeDto
    {
        [XmlAttribute]
        public string Name { get; set; }

        public string Source { get; set; }

        public string Text { get; set; }
    }
}
