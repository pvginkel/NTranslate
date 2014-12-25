using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate
{
    public class TranslationString
    {
        public string Prolog { get; private set; }
        public string Text { get; private set; }
        public string Epilog { get; private set; }

        public static TranslationString Parse(string text)
        {
            if (text == null)
                return null;

            if (text.Length == 0)
                return new TranslationString(String.Empty, String.Empty, String.Empty);

            int start = 0;
            while (start < text.Length && IsExtra(text[start]))
            {
                start++;
            }

            int end = text.Length - 1;
            while (end >= 0 && IsExtra(text[end]))
            {
                end--;
            }

            if (start > end)
                return new TranslationString(text, String.Empty, String.Empty);

            return new TranslationString(
                text.Substring(0, start),
                text.Substring(start, end - start + 1),
                text.Substring(end + 1)
            );
        }

        private static bool IsExtra(char c)
        {
            // Characters that are part of a range are not extra characters.
            // This would otherwise break up e.g. quoted strings.

            switch (c)
            {
                case '\'':
                case '"':
                case '{':
                case '}':
                case '[':
                case ']':
                case '<':
                case '>':
                case '&': // Mnemonics may move
                    return false;

                default:
                    return !Char.IsLetterOrDigit(c);
            }
        }

        private TranslationString(string prolog, string text, string epilog)
        {
            Prolog = prolog;
            Text = text;
            Epilog = epilog;
        }

        public override string ToString()
        {
            return Prolog + Text + Epilog;
        }
    }
}
