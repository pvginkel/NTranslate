using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NTranslate.Dto;

namespace NTranslate
{
    public static class TranslationUtil
    {
        public static bool ShouldHide(NodeDto node)
        {
            if (node == null)
                throw new ArgumentNullException("node");

            return
                String.IsNullOrEmpty(node.Text) &&
                ShouldHide(node.Name, node.Source);
        }

        public static bool ShouldHide(string name, string translation)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            return
                name.StartsWith(">>") ||
                String.IsNullOrEmpty(translation) ||
                (translation.StartsWith("<<") && translation.EndsWith(">>"));
        }
    }
}
