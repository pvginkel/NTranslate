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
        private readonly Dictionary<Type, object> _properties = new Dictionary<Type, object>(); 

        public string Name { get; private set; }
        public string FileName { get; private set; }
        public bool IsDirectory { get; private set; }
        public ProjectItemCollection Children { get; private set; }
        public TreeNode TreeNode { get; private set; }

        public ProjectItem Parent { get; internal set; }

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
            : this(fileName, null, directory)
        {
        }

        public ProjectItem(string fileName, string name, bool directory)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            FileName = fileName;
            Name = name ?? Path.GetFileName(fileName);
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
            var document = GetProperty<IDocument>();
            if (document != null)
            {
                document.Show();
                return;
            }

            document = Program.DocumentManager.CreateDocument(this);
            if (document == null)
                return;

            SetProperty<IDocument>(document);

            document.Show();
        }

        public void CloseDocument()
        {
            var document = GetProperty<IDocument>();
            if (document == null)
                return;

            document.Close();

            SetProperty<IDocument>(null);
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

        public T GetProperty<T>()
            where T : class
        {
            object value;
            _properties.TryGetValue(typeof(T), out value);
            return (T)value;
        }

        public void SetProperty<T>(object value)
            where T : class
        {
            if (value == null)
                _properties.Remove(typeof(T));
            else if (!(value is T))
                throw new ArgumentException("Expected value to be of type " + typeof(T));
            else
                _properties[typeof(T)] = value;
        }
    }
}
