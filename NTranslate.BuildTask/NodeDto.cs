using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate.BuildTask
{
    internal class NodeDto
    {
        public string Name { get; private set; }
        public string Text { get; private set; }

        public NodeDto(string name, string text)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (text == null)
                throw new ArgumentNullException("text");

            Name = name;
            Text = text;
        }
    }
}
