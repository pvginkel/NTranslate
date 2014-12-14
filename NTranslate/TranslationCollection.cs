using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace NTranslate
{
    public class TranslationCollection : KeyedCollection<CultureInfo, TranslationFile>
    {
        protected override CultureInfo GetKeyForItem(TranslationFile item)
        {
            return item.Language;
        }
    }
}
