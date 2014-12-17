using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NTranslate.Dto;

namespace NTranslate
{
    public partial class ResourceNodeControl : UserControl
    {
        private static readonly Color Green = Color.FromArgb(200, 255, 200);
        private static readonly Color Orange = Color.FromArgb(255, 234, 194);
        private static readonly Color Red = Color.FromArgb(255, 200, 200);

        public string Source
        {
            get { return _source.Text; }
        }

        public string Translated
        {
            get { return _translated.Text; }
        }

        public ResourceNodeState State { get; private set; }

        public event EventHandler Changed;

        protected virtual void OnChanged(EventArgs e)
        {
            var handler = Changed;
            if (handler != null)
                handler(this, e);
        }

        public ResourceNodeControl()
        {
            InitializeComponent();
        }

        public ResourceNodeControl(FileNode node, TranslationDictionary dictionary)
            : this()
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            _name.Text = node.Name;
            _source.Text = node.Source;
            _comment.Text = node.Comment;
            _originalSource.Text = node.OriginalSource;
            _translated.Text = node.Translated;
            _proposal.Text = dictionary.GetTranslation(node.Source);

            UpdateControlVisibility();
            UpdateColor();
        }

        private void UpdateControlVisibility()
        {
            _tableLayoutPanel.SuspendLayout();

            _originalSourceLabel.Visible = _source.Text != _originalSource.Text;
            _originalSource.Visible = _source.Text != _originalSource.Text;

            _commentLabel.Visible = _comment.Text.Length > 0;
            _comment.Visible = _comment.Text.Length > 0;

            _proposalLabel.Visible = _translated.Text != _proposal.Text;
            _proposal.Visible = _translated.Text != _proposal.Text;
            _applyButton.Visible = _translated.Text != _proposal.Text;

            _tableLayoutPanel.ResumeLayout();
        }

        public void UpdateAfterSave()
        {
            _originalSource.Text = _source.Text;

            UpdateControlVisibility();
            UpdateColor();
        }

        public NodeDto CreateNode()
        {
            return new NodeDto
            {
                Name = _name.Text,
                Source = _source.Text,
                Text = _translated.Text
            };
        }

        private void UpdateColor()
        {
            if (_translated.Text.Length == 0)
            {
                _panel.BackColor = Red;
                State = ResourceNodeState.Missing;
            }
            else if (_source.Text != _originalSource.Text)
            {
                _panel.BackColor = Orange;
                State = ResourceNodeState.ConflictingSource; ;
            }
            else
            {
                _panel.BackColor = Green;
                State = ResourceNodeState.Complete; ;
            }

            _name.BackColor = _panel.BackColor;
        }

        private void _translated_TextChanged(object sender, EventArgs e)
        {
            OnChanged(EventArgs.Empty);

            UpdateColor();
        }

        private void _tableLayoutPanel_SizeChanged(object sender, EventArgs e)
        {
            _panel.Height = _tableLayoutPanel.Height + _panel.Padding.Vertical;
            Height = _panel.Height + 1;
        }

        private void _proposal_SizeChanged(object sender, EventArgs e)
        {
            _applyButton.Height = _proposal.Height;
        }

        private void _applyButton_Click(object sender, EventArgs e)
        {
            _translated.Text = _proposal.Text;

            UpdateControlVisibility();
        }

        private void _translated_Leave(object sender, EventArgs e)
        {
            if (_translated.Text.Trim().Length <= 0)
                return;

            var source = TranslationString.Parse(_source.Text);
            var translated = TranslationString.Parse(_translated.Text);

            var text = source.Prolog + translated.Text + source.Epilog;

            if (text == _translated.Text)
                return;

            _translated.Text = text;

            OnChanged(EventArgs.Empty);

            UpdateControlVisibility();
            UpdateColor();
        }
    }
}
