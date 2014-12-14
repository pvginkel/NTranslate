using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NTranslate
{
    public class ProjectItem
    {
        private ProjectItemState _state;
        public string Name { get; private set; }
        public string FileName { get; private set; }
        public bool IsDirectory { get; private set; }
        public ProjectItemCollection Children { get; private set; }
        public TreeNode TreeNode { get; private set; }
        public IDocument Document { get; private set; }

        public ProjectItemState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                {
                    _state = value;
                    UpdateImage();
                }
            }
        }

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

            UpdateImage();
        }

        public void OpenDocument()
        {
            if (Document != null)
            {
                Document.Show();
                return;
            }

            Document = Program.DocumentManager.CreateDocument(this);
            if (Document == null)
                return;

            Document.Show();
        }

        public void CloseDocument()
        {
            if (Document == null)
                return;

            Document.Close();
            Document = null;
        }

        private void UpdateImage()
        {
            if (IsDirectory)
            {
                TreeNode.ImageIndex = Program.MainForm.ProjectExplorer.FolderImageIndex;
            }
            else
            {
                OverlayImage overlay;
                switch (_state)
                {
                    case ProjectItemState.Complete:
                        overlay = OverlayImage.Complete;
                        break;

                    case ProjectItemState.Incomplete:
                        overlay = OverlayImage.Incomplete;
                        break;

                    default:
                        overlay = OverlayImage.None;
                        break;
                }

                TreeNode.ImageIndex = Program.MainForm.ProjectExplorer.GetImageIndex(Path.GetExtension(FileName), overlay);
            }

            TreeNode.SelectedImageIndex = TreeNode.ImageIndex;
        }
    }
}
