using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate
{
    public class ProjectManager
    {
        private Project _currentProject;

        public event EventHandler CurrentProjectChanged;

        protected virtual void OnCurrentProjectChanged(EventArgs e)
        {
            var handler = CurrentProjectChanged;
            if (handler != null)
                handler(this, e);
        }

        public Project CurrentProject
        {
            get { return _currentProject; }
            private set
            {
                if (_currentProject != value)
                {
                    _currentProject = value;
                    OnCurrentProjectChanged(EventArgs.Empty);
                }
            }
        }

        public void OpenProject(string fileName)
        {
            if (_currentProject != null && !CloseProject())
                return;

            CurrentProject = new Project(fileName);
        }

        public bool CloseProject()
        {
            if (CurrentProject == null)
                return true;

            var documents = Program.MainForm.DockPanel.Documents.Select(p => (IDocument)p).ToList();

            if (documents.Any(p => p.IsDirty))
            {
                var result = MessageBox.Show(
                    Program.MainForm,
                    "Do you want to save your changes?",
                    "Save changes",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning
                );

                if (result == DialogResult.Cancel)
                    return false;

                if (result == DialogResult.Yes)
                {
                    foreach (var document in documents)
                    {
                        document.Save();
                    }
                }
            }

            CurrentProject.Dispose();
            CurrentProject = null;

            return true;
        }
    }
}
