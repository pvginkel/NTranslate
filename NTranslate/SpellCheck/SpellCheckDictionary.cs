using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace NTranslate.SpellCheck
{
    internal class SpellCheckDictionary
    {
        public string Code { get; private set; }
        public string Label { get; private set; }
        public bool IsActive { get; private set; }

        public SpellCheckDictionary(string code, bool active)
        {
            if (code == null)
                throw new ArgumentNullException("code");

            Code = code;
            Label = CultureInfo.GetCultureInfo(code).DisplayName;
            IsActive = active;
        }
    }
}