using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate
{
    public class FileNode
    {
        public string Name { get; private set; }
        public string Source { get; private set; }
        public string OriginalSource { get; private set; }
        public string Translated { get; private set; }
        public string Comment { get; private set; }

        public bool Hidden
        {
            get { return TranslationUtil.ShouldHide(Name, Source); }
        }

        public FileNode(string name, string source, string originalSource, string translated, string comment)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            Name = name;
            if (!String.IsNullOrEmpty(source))
                Source = source;
            if (!String.IsNullOrEmpty(originalSource))
                OriginalSource = originalSource;
            if (!String.IsNullOrEmpty(translated))
                Translated = translated;
            if (!String.IsNullOrEmpty(comment))
                Comment = comment;
        }
    }
}
