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

        public ResourceNodeControl(FileNode node)
            : this()
        {
            _name.Text = node.Name;
            _source.Text = node.Source;
            _comment.Text = node.Comment;
            _originalSource.Text = node.OriginalSource;
            _translated.Text = node.Translated;

            UpdateControlVisibility();
            UpdateColor();
        }

        private void UpdateControlVisibility()
        {
            if (_source.Text == _originalSource.Text)
            {
                _tableLayoutPanel.Controls.Remove(_originalSourceLabel);
                _tableLayoutPanel.Controls.Remove(_originalSource);
            }

            if (_comment.Text.Length == 0)
            {
                _tableLayoutPanel.Controls.Remove(_commentLabel);
                _tableLayoutPanel.Controls.Remove(_comment);
            }
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
    }
}
