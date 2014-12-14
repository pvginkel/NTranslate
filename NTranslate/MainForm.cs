using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace NTranslate
{
    public partial class MainForm : SystemEx.Windows.Forms.Form
    {
        private readonly string[] _args;
        private readonly string _loadedTitle;
        private CultureInfo _language;

        public ProjectExplorerForm ProjectExplorer { get; private set; }

        public CultureInfo Language
        {
            get { return _language; }
            private set
            {
                if (!Equals(_language, value))
                {
                    _language = value;
                    OnLanguageChanged(EventArgs.Empty);
                }
            }
        }

        public event EventHandler LanguageChanged;

        protected virtual void OnLanguageChanged(EventArgs e)
        {
            var handler = LanguageChanged;
            if (handler != null)
                handler(this, e);
        }

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

            ProjectExplorer = new ProjectExplorerForm(this);
            ProjectExplorer.Show(_dockPanel);
            ProjectExplorer.SelectedProjectItemChanged += ProjectExplorer_SelectedProjectItemChanged;

            using (var key = Program.BaseKey)
            {
                string language = key.GetValue("Language") as string;
                if (language != null)
                    _language = CultureInfo.GetCultureInfo(language);
            }
        }

        void ProjectExplorer_SelectedProjectItemChanged(object sender, EventArgs e)
        {
            UpdateEnabled();
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

            RebuildLanguages();

            UpdateEnabled();
        }

        private void UpdateEnabled()
        {
            bool haveProject = Program.ProjectManager.CurrentProject != null;
            bool haveDocument = Program.DocumentManager.CurrentDocument != null;

            openToolStripMenuItem.Enabled = ProjectExplorer.SelectedProjectItem != null;
            closeProjectToolStripMenuItem.Enabled = haveProject;
            saveToolStripMenuItem.Enabled = haveDocument;
            saveAllToolStripMenuItem.Enabled = haveProject;
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

        private void addLanguageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new AddLanguageForm())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Program.ProjectManager.CurrentProject.AddLanguage(form.SelectedLanguage);

                    SelectLanguage(form.SelectedLanguage);
                }
            }
        }

        private void RebuildLanguages()
        {
            // Remove the existing language menu items.

            while (!(languageToolStripMenuItem.DropDownItems[0] is ToolStripSeparator))
            {
                languageToolStripMenuItem.DropDownItems.RemoveAt(0);
            }

            var separator = (ToolStripSeparator)languageToolStripMenuItem.DropDownItems[0];

            int offset = 0;
            bool hadLanguage = false;

            if (Program.ProjectManager.CurrentProject != null)
            {
                var languages = Program.ProjectManager.CurrentProject.Translations
                    .Select(p => p.Language)
                    .OrderBy(p => p.DisplayName);

                foreach (var language in languages)
                {
                    var menuItem = new ToolStripMenuItem
                    {
                        Text = language.DisplayName,
                        Tag = language,
                        Checked = language.Equals(_language)
                    };

                    hadLanguage = hadLanguage || menuItem.Checked;

                    menuItem.Click += languageMenuItem_Click;

                    languageToolStripMenuItem.DropDownItems.Insert(
                        offset++,
                        menuItem
                    );
                }
            }

            if (!hadLanguage)
                _language = null;

            separator.Visible = offset > 0;
        }

        void languageMenuItem_Click(object sender, EventArgs e)
        {
            SelectLanguage((CultureInfo)((ToolStripMenuItem)sender).Tag);
        }

        private void SelectLanguage(CultureInfo language)
        {
            var documents = _dockPanel.Documents.Select(p => (IDocument)p).ToList();

            if (documents.Any(p => p.IsDirty))
            {
                var result = MessageBox.Show(
                    this,
                    "To change the language, all open documents must be saved. Do you want to continue?",
                    "Change language",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning
                    );

                if (result != DialogResult.Yes)
                    return;

                foreach (var document in documents)
                {
                    document.Save();
                }
            }

            Language = language;

            using (var key = Program.BaseKey)
            {
                key.SetValue("Language", language.Name);
            }

            RebuildLanguages();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.UserClosing)
                return;

            if (!Program.ProjectManager.CloseProject())
                e.Cancel = true;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProjectExplorer.SelectedProjectItem.OpenDocument();
        }
    }
}
