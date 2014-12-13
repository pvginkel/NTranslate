using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace NTranslate.App
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly string[] _args;
        private readonly string _loadedTitle;

        public ProjectExplorerForm ProjectExplorer { get; private set; }

        public DockPanel DockPanel
        {
            get { return _dockPanel; }
        }

        public MainForm(string[] args)
        {
            if (args == null)
                throw new ArgumentNullException("args");

            _args = args;

            InitializeComponent();

            _loadedTitle = Text;

            _dockPanel.Theme = new VS2012LightTheme();

            ProjectExplorer = new ProjectExplorerForm();
            ProjectExplorer.Show(_dockPanel);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            Program.ProjectManager.CurrentProjectChanged += ProjectManager_CurrentProjectChanged;
            Program.DocumentManager.CurrentDocumentChanged += DocumentManager_CurrentDocumentChanged;

            UpdateEnabled();
        }

        void DocumentManager_CurrentDocumentChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
        }

        void ProjectManager_CurrentProjectChanged(object sender, EventArgs e)
        {
            var project = Program.ProjectManager.CurrentProject;

            Text =
                project == null
                ? _loadedTitle
                : _loadedTitle + " - " + project.RootNode.Name;

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            bool haveProject = Program.ProjectManager.CurrentProject != null;
            bool haveDocument = Program.DocumentManager.CurrentDocument != null;

            closeProjectToolStripMenuItem.Enabled = haveProject;
            saveToolStripMenuItem.Enabled = haveDocument;
            closeToolStripMenuItem.Enabled = haveDocument;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new OpenFileDialog())
            {
                form.Filter = "C# Project (*.csproj)|*.csproj|All Files (*.*)|All Files";
                if (form.ShowDialog(this) == DialogResult.OK)
                    Program.ProjectManager.OpenProject(form.FileName);
            }
        }

        private void MainForm_Shown(object sender, EventArgs e)
        {
            foreach (string arg in _args)
            {
                if (String.Equals(".csproj", Path.GetExtension(arg), StringComparison.OrdinalIgnoreCase))
                {
                    Program.ProjectManager.OpenProject(arg);
                    break;
                }
            }
        }

        private void closeProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.ProjectManager.CloseProject();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.DocumentManager.CurrentDocument.Save();
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (IDocument content in _dockPanel.Documents)
            {
                content.Save();
            }
        }
    }
}
