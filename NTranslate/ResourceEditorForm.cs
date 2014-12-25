using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NTranslate.Dto;

namespace NTranslate
{
    public partial class ResourceEditorForm : DocumentForm
    {
        private Selection _selection = Selection.Pending;

        public ResourceEditorForm(ProjectItem projectItem)
            : base(projectItem)
        {
            InitializeComponent();

            Text = ProjectItem.Name;

            Program.MainForm.LanguageChanged += MainForm_LanguageChanged;

            Disposed += ResourceEditorForm_Disposed;
        }

        public override void Save()
        {
            var nodes = new List<NodeDto>();

            foreach (ResourceNodeControl control in _tableLayoutPanel.Controls)
            {
                nodes.Add(control.CreateNode());
            }

            Project.FindProject(ProjectItem).SaveTranslations(Program.MainForm.Language, ProjectItem, nodes);

            foreach (ResourceNodeControl control in _tableLayoutPanel.Controls)
            {
                control.UpdateAfterSave();
            }

            IsDirty = false;
        }

        private void ReloadNodes()
        {
            _tableLayoutPanel.SuspendLayout();

            var translations = Project.FindProject(ProjectItem).LoadTranslations(Program.MainForm.Language, ProjectItem);
            var contents = FileContents.Load(ProjectItem, translations);

            foreach (var node in _tableLayoutPanel.Controls.Cast<Control>().ToList())
            {
                node.Dispose();
            }

            _tableLayoutPanel.RowStyles.Clear();
            _tableLayoutPanel.RowCount = 1;

            var dictionary = Program.SolutionManager.CurrentSolution.Dictionary;

            foreach (var node in contents.Nodes)
            {
                AddNode(node, dictionary);
            }

            SetSelection(_selection, false);

            _tableLayoutPanel.ResumeLayout();
        }

        private void SetSelection(Selection selection, bool showHidden)
        {
            SetSelection(selection | (showHidden ? Selection.Hidden : 0));
        }

        private void SetSelection(Selection selection)
        {
            _tableLayoutPanel.SuspendLayout();

            _selection = selection;

            _showAll.Checked = (_selection & Selection.All) != 0;
            _showPending.Checked = (_selection & Selection.Pending) != 0;
            _showMissing.Checked = (_selection & Selection.Missing) != 0;
            _showHidden.Checked = (_selection & Selection.Hidden) != 0;

            foreach (ResourceNodeControl control in _tableLayoutPanel.Controls)
            {
                if ((_selection & Selection.Hidden) == 0 && TranslationUtil.ShouldHide(control.CreateNode()))
                {
                    control.Visible = false;
                }
                else
                {
                    switch (_selection & ~Selection.Hidden)
                    {
                        case Selection.All:
                            control.Visible = true;
                            break;

                        case Selection.Pending:
                            control.Visible = control.State == ResourceNodeState.ConflictingSource || control.State == ResourceNodeState.Missing;
                            break;

                        case Selection.Missing:
                            control.Visible = control.State == ResourceNodeState.Missing;
                            break;
                    }
                }
            }

            _tableLayoutPanel.ResumeLayout();
        }

        void MainForm_LanguageChanged(object sender, EventArgs e)
        {
            ReloadNodes();
        }

        void ResourceEditorForm_Disposed(object sender, EventArgs e)
        {
            Program.MainForm.LanguageChanged -= MainForm_LanguageChanged;
        }

        private void AddNode(FileNode node, TranslationDictionary dictionary)
        {
            var editor = new ResourceNodeControl(node, dictionary)
            {
                Dock = DockStyle.Fill
            };

            editor.Changed += editor_Changed;
            editor.Visible = false;

            _tableLayoutPanel.RowCount = _tableLayoutPanel.Controls.Count + 1;

            while (_tableLayoutPanel.RowStyles.Count <= _tableLayoutPanel.RowCount)
            {
                _tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            _tableLayoutPanel.SetRow(editor, _tableLayoutPanel.Controls.Count - 1);
            _tableLayoutPanel.Controls.Add(editor);
        }

        void editor_Changed(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void _showAll_Click(object sender, EventArgs e)
        {
            SetSelection(Selection.All, _showHidden.Checked);
        }

        private void _showPending_Click(object sender, EventArgs e)
        {
            SetSelection(Selection.Pending, _showHidden.Checked);
        }

        private void _showMissing_Click(object sender, EventArgs e)
        {
            SetSelection(Selection.Missing, _showHidden.Checked);
        }

        private void _showHidden_Click(object sender, EventArgs e)
        {
            SetSelection(_selection & ~Selection.Hidden, (_selection & Selection.Hidden) == 0);
        }

        private void ResourceEditorForm_Shown(object sender, EventArgs e)
        {
            ReloadNodes();
        }

        [Flags]
        private enum Selection
        {
            All = 1,
            Pending = 2,
            Missing = 4,
            Hidden = 8
        }
    }
}
