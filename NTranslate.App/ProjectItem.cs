using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate.App
{
    public class ProjectItem
    {
        public string Name { get; private set; }
        public string FileName { get; private set; }
        public bool IsDirectory { get; private set; }
        public ProjectItemCollection Children { get; private set; }
        public TreeNode TreeNode { get; private set; }
        public IDocument Document { get; private set; }

        public ProjectItem(string fileName, bool directory)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileName = fileName;
            Name = Path.GetFileName(fileName);
            IsDirectory = directory;
            Children = new ProjectItemCollection(this);

            TreeNode = new TreeNode(Name)
            {
                Text = Name,
                Tag = this
            };

            if (directory)
                TreeNode.ImageIndex = Program.MainForm.ProjectExplorer.FolderImageIndex;
            else
                TreeNode.ImageIndex = Program.MainForm.ProjectExplorer.GetImageIndex(Path.GetExtension(fileName));

            TreeNode.SelectedImageIndex = TreeNode.ImageIndex;
        }

        public void OpenDocument()
        {
            if (Document != null)
            {
                Document.Show();
                return;
            }

            Document = Program.DocumentManager.CreateDocument(this);
            Document.Show();
        }

        public void CloseDocument()
        {
            if (Document == null)
                return;

            Document.Close();
            Document = null;
        }
    }
}
