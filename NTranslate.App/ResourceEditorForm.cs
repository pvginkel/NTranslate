using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate.App
{
    public partial class ResourceEditorForm : DocumentForm
    {
        public ResourceEditorForm(ProjectItem projectItem)
            : base(projectItem)
        {
            InitializeComponent();
        }
    }
}
