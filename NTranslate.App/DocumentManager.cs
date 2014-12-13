using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NTranslate.App
{
    public class DocumentManager
    {
        private IDocument _currentDocument;

        public IDocument CurrentDocument
        {
            get { return _currentDocument; }
            private set
            {
                if (_currentDocument != value)
                {
                    _currentDocument = value;
                    OnCurrentDocumentChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler CurrentDocumentChanged;

        protected virtual void OnCurrentDocumentChanged(EventArgs e)
        {
            var handler = CurrentDocumentChanged;
            if (handler != null)
                handler(this, e);
        }

        public DocumentManager()
        {
            Program.MainForm.DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
        }

        void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            CurrentDocument = Program.MainForm.DockPanel.ActiveContent as IDocument;
        }

        public IDocument CreateDocument(ProjectItem projectItem)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            switch (Path.GetExtension(projectItem.FileName).ToLowerInvariant())
            {
                case ".resx":
                    return new ResourceEditorForm(projectItem);

                default:
                    throw new ArgumentOutOfRangeException("projectItem");
            }
        }
    }
}
