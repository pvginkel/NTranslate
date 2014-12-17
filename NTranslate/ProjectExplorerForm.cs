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

namespace NTranslate
{
    public partial class ProjectExplorerForm : DockContent
    {
        private readonly MainForm _mainForm;
        public int FolderImageIndex { get; private set; }

        private readonly ImageListManager _imageList;
        private readonly Dictionary<string, int> _imageCache = new Dictionary<string, int>();
        private readonly Dictionary<OverlayImage, int> _overlays = new Dictionary<OverlayImage, int>();

        public ProjectItem SelectedProjectItem
        {
            get
            {
                if (_treeView.SelectedNode != null)
                    return (ProjectItem)_treeView.SelectedNode.Tag;

                return null;
            }
        }

        public event EventHandler SelectedProjectItemChanged;

        protected virtual void OnSelectedProjectItemChanged(EventArgs e)
        {
            var handler = SelectedProjectItemChanged;
            if (handler != null)
                handler(this, e);
        }

        public ProjectExplorerForm(MainForm mainForm)
        {
            if (mainForm == null)
                throw new ArgumentNullException("mainForm");

            _mainForm = mainForm;

            InitializeComponent();

            VisualStyleUtil.StyleTreeView(_treeView);

            _imageList = new ImageListManager();
            _treeView.ImageList = _imageList.ImageList;

            _overlays.Add(OverlayImage.Incomplete, _treeView.ImageList.Images.Count);
            _imageList.ImageList.Images.Add(NeutralResources.IncompleteOverlay);
            _overlays.Add(OverlayImage.Complete, _treeView.ImageList.Images.Count);
            _imageList.ImageList.Images.Add(NeutralResources.CompleteOverlay);

            FolderImageIndex = GetFolderImageIndex();

            Program.SolutionManager.CurrentSolutionChanged += SolutionManager_CurrentSolutionChanged;
            mainForm.DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
        }

        void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            var document = _mainForm.DockPanel.ActiveDocument as IDocument;
            if (document == null)
                return;

            _treeView.SelectedNode = document.ProjectItem.TreeNode;
        }

        void SolutionManager_CurrentSolutionChanged(object sender, EventArgs e)
        {
            _treeView.Nodes.Clear();

            var solution = Program.SolutionManager.CurrentSolution;
            if (solution != null)
            {
                _treeView.Nodes.Add(solution.RootNode.TreeNode);
                _treeView.Nodes[0].Expand();
            }
        }

        private int GetFolderImageIndex()
        {
            string tempPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            try
            {
                Directory.CreateDirectory(tempPath);

                return _imageList.GetIndexForDirectory(tempPath);
            }
            finally
            {
                Directory.Delete(tempPath);
            }
        }

        public int GetImageIndex(string extension, OverlayImage overlay)
        {
            if (extension == null)
                throw new ArgumentNullException("extension");

            extension = extension.ToLowerInvariant();

            int imageIndex;
            if (!_imageCache.TryGetValue(extension, out imageIndex))
            {
                imageIndex = _imageList.GetIndexForFileName("dummy" + extension);
                _imageCache.Add(extension, imageIndex);
            }

            int overlayIndex;
            if (_overlays.TryGetValue(overlay, out overlayIndex))
                return _imageList.GetIndexForOverlay(imageIndex, overlayIndex);

            return imageIndex;
        }

        private void _treeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var projectItem = (ProjectItem)_treeView.SelectedNode.Tag;
            if (projectItem.IsDirectory)
                return;

            projectItem.OpenDocument();
        }

        private void _treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            OnSelectedProjectItemChanged(EventArgs.Empty);
        }
    }
}
