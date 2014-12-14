using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate.SpellCheck
{
    internal partial class AddDictionariesForm : SystemEx.Windows.Forms.Form
    {
        private readonly SpellCheck _spellCheck;

        public AddDictionariesForm(SpellCheck spellCheck)
        {
            _spellCheck = spellCheck;
            InitializeComponent();
        }

        private void _webBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (e.Url.LocalPath.EndsWith(".xpi"))
            {
                _spellCheck.AddDictionary(this, e.Url);

                e.Cancel = true;
                Dispose();
            }
        }
    }
}
