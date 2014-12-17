using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate
{
    public class TranslationDictionary
    {
        private readonly Dictionary<string, List<string>> _dictionary = new Dictionary<string, List<string>>();

        public void Add(string source, string target)
        {
            if (source == null)
                throw new ArgumentNullException("source");
            if (target == null)
                throw new ArgumentNullException("target");

            var sourceString = TranslationString.Parse(source);
            var targetString = TranslationString.Parse(target);

            if (sourceString.Text.Length == 0 || targetString.Text.Length == 0)
                return;

            List<string> translations;
            if (!_dictionary.TryGetValue(sourceString.Text, out translations))
            {
                translations = new List<string>();
                _dictionary.Add(sourceString.Text, translations);
            }

            if (!translations.Contains(targetString.Text))
                translations.Add(targetString.Text);
        }

        public string GetTranslation(string source)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var sourceString = TranslationString.Parse(source);
            if (sourceString.Text.Length == 0)
                return null;

            List<string> translations;
            if (
                !_dictionary.TryGetValue(sourceString.Text, out translations) ||
                translations.Count != 1
            )
                return null;

            var translation = translations[0];

            return sourceString.Prolog + translation + sourceString.Epilog;
        }
    }
}
