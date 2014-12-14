using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NTranslate.SpellCheck
{
    partial class SpellCheckTextBox
    {
        private struct Word
        {
            private readonly CharacterRange _range;
            private readonly string _text;

            public bool IsEmpty
            {
                get { return _text == null; }
            }

            public CharacterRange Range
            {
                get { return _range; }
            }

            public string Text
            {
                get { return _text; }
            }

            public Word(CharacterRange range, string text)
            {
                if (text == null)
                    throw new ArgumentNullException("text");

                Debug.Assert(text.Length > 0);

                _range = range;
                _text = text;
            }

            public override string ToString()
            {
                return String.Format("{0} ({1}) {2}", Text, _range.First, _range.Length);
            }

            public static Word GetWordFromPosition(string text, int position, bool adjustEnd)
            {
                if (text.Length == 0)
                    return new Word();

                int start = GetWordStart(text, position, adjustEnd);
                if (start < 0)
                    return new Word();

                int length = GetWordEnd(text, start) - start;

                if (length == 0)
                    return new Word();

                text = RemoveWhitespace(text.Substring(start, length));

                return new Word(new CharacterRange(start, length), text);
            }

            private static string RemoveWhitespace(string text)
            {
                StringBuilder sb = null;

                for (int i = 0; i < text.Length; i++)
                {
                    if (Char.IsWhiteSpace(text[i]))
                    {
                        if (sb == null)
                        {
                            sb = new StringBuilder();
                            if (i > 0)
                                sb.Append(text, 0, i);
                        }
                    }
                    else if (sb != null)
                    {
                        sb.Append(text[i]);
                    }
                }

                if (sb != null)
                    return sb.ToString();

                return text;
            }

            private static int GetWordStart(string text, int position, bool adjustEnd)
            {
                if (position == text.Length)
                    position--;

                int wordStart = position;

                if (!Char.IsLetterOrDigit(text[wordStart]) && wordStart > 0)
                {
                    if (!adjustEnd)
                        return -1;

                    wordStart--;
                }

                for (int i = wordStart; i >= 0; i--)
                {
                    if (!Char.IsLetterOrDigit(text[i]))
                        break;

                    wordStart = i;
                }

                return wordStart;
            }

            private static int GetWordEnd(string text, int position)
            {
                for (int i = position; i < text.Length; i++)
                {
                    if (!Char.IsLetterOrDigit(text[i]))
                        return i;
                }

                return text.Length;
            }
        }
    }
}
