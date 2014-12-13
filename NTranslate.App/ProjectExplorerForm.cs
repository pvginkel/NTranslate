using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SystemEx.Windows.Forms;

namespace NTranslate.App
{
    public partial class ProjectExplorerForm : DockContent
    {
        public int FolderImageIndex { get; private set; }

        private readonly SystemImageList _imageList;
        private readonly Dictionary<string, int> _imageCache = new Dictionary<string, int>(); 

        public ProjectExplorerForm()
        {
            InitializeComponent();

            VisualStyleUtil.StyleTreeView(_treeView);

            _imageList = new SystemImageList();
            _imageList.Assign(_treeView);

            FolderImageIndex = GetFolderImageIndex();

            Program.ProjectManager.CurrentProjectChanged += ProjectManager_CurrentProjectChanged;
        }

        void ProjectManager_CurrentProjectChanged(object sender, EventArgs e)
        {
            _treeView.Nodes.Clear();

            var project = Program.ProjectManager.CurrentProject;
            if (project != null)
            {
                _treeView.Nodes.Add(project.RootNode.TreeNode);
                _treeView.Nodes[0].Expand();
            }
        }

        private int GetFolderImageIndex()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            try
            {
                Directory.CreateDirectory(tempPath);

                return _imageList.AddShellIcon(tempPath, 0);
            }
            finally
            {
                Directory.Delete(tempPath);
            }
        }

        public int GetImageIndex(string extension)
        {
            if (extension == null)
                throw new ArgumentNullException("extension");

            extension = extension.ToLowerInvariant();

            int imageIndex;
            if (!_imageCache.TryGetValue(extension, out imageIndex))
            {
                imageIndex = _imageList.AddShellIcon(
                    "dummy" + extension,
                    ShellIconType.UseFileAttributes
                );
                _imageCache.Add(extension, imageIndex);
            }

            return imageIndex;
        }

        private void _treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var projectItem = (ProjectItem)_treeView.SelectedNode.Tag;
            if (projectItem.IsDirectory)
                return;

            projectItem.OpenDocument();
        }
    }
}
