using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NTranslate.App
{
    public class TranslationCollection : KeyedCollection<string, TranslationFile>
    {
        protected override string GetKeyForItem(TranslationFile item)
        {
            return item.Language;
        }
    }
}
