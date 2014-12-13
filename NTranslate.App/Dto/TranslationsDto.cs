﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace NTranslate.App.Dto
{
    [XmlType("Translations", Namespace = Ns)]
    public class TranslationsDto
    {
        public const string Ns = "https://github.com/pvginkel/NTranslate/Translations";

        [XmlAttribute]
        public string Language { get; set; }

        public List<FileDto> Files { get; set; }

        public TranslationsDto()
        {
            Files = new List<FileDto>();
        }
    }
}
