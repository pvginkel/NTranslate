using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate
{
    public class SolutionManager
    {
        private Solution _solution;

        public Solution CurrentSolution
        {
            get { return _solution; }
            private set
            {
                if (_solution != value)
                {
                    _solution = value;
                    OnCurrentSolutionChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler CurrentSolutionChanged;

        protected virtual void OnCurrentSolutionChanged(EventArgs e)
        {
            var handler = CurrentSolutionChanged;
            if (handler != null)
                handler(this, e);
        }

        public void OpenSolution(string fileName)
        {
            if (CurrentSolution != null && !CloseSolution())
                return;

            CurrentSolution = new Solution(fileName);
        }

        public bool CloseSolution()
        {
            if (CurrentSolution == null)
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

            CurrentSolution.Dispose();
            CurrentSolution = null;

            return true;
        }
    }
}
