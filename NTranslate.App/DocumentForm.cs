using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SystemEx.Windows.Forms;

namespace NTranslate.App
{
    public partial class DocumentForm : DockContent, IDocument
    {
        private bool _closing;
        private bool _isDirty;

        public ProjectItem ProjectItem { get; private set; }

        public bool IsDirty
        {
            get { return _isDirty; }
            set
            {
                if (_isDirty != value)
                {
                    _isDirty = value;
                    Text = ProjectItem.Name + (value ? " *" : "");
                }
            }
        }

        public DocumentForm()
        {
        }

        public DocumentForm(ProjectItem projectItem)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            ProjectItem = projectItem;

            Text = ProjectItem.Name;

            InitializeComponent();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_closing)
                return;

            _closing = true;

            base.OnClosed(e);

            ProjectItem.CloseDocument();
        }

        public new void Show()
        {
            Show(Program.MainForm.DockPanel);
        }

        public virtual void Save()
        {
            throw new InvalidOperationException();
        }
    }
}
