using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate
{
    public partial class AddLanguageForm : SystemEx.Windows.Forms.Form
    {
        public CultureInfo SelectedLanguage
        {
            get
            {
                var item = (ComboBoxItem)_language.SelectedItem;
                return item != null ? item.CultureInfo : null;
            }
        }

        public AddLanguageForm()
        {
            InitializeComponent();

            foreach (var cultureInfo in CultureInfo.GetCultures(CultureTypes.AllCultures).OrderBy(p => p.DisplayName))
            {
                if (!String.IsNullOrEmpty(cultureInfo.Name))
                    _language.Items.Add(new ComboBoxItem(cultureInfo));
            }

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            _acceptButton.Enabled = _language.SelectedItem != null;
        }

        private class ComboBoxItem
        {
            public CultureInfo CultureInfo { get; private set; }

            public ComboBoxItem(CultureInfo cultureInfo)
            {
                CultureInfo = cultureInfo;
            }

            public override string ToString()
            {
                return CultureInfo.DisplayName + " - " + CultureInfo.Name;
            }
        }

        private void _language_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        private void _acceptButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}
