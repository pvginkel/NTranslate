using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NTranslate.App.Dto;

namespace NTranslate.App
{
    public partial class ResourceNodeControl : UserControl
    {
        public NodeDto Node { get; private set; }

        public ResourceNodeControl(NodeDto node, string sourceText)
        {
            Node = node;

            InitializeComponent();

            _name.Text = node.Name;
            _source.Text = sourceText;
            _originalSource.Text = node.Source;
            _translated.Text = node.Text;

            if (_source.Text == _originalSource.Text)
            {
                Controls.Remove(_originalSourceLabel);
                Controls.Remove(_originalSource);
            }
        }
    }
}
