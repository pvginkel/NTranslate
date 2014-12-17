using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NTranslate
{
    public class ProjectItemCollection : Collection<ProjectItem>
    {
        private readonly ProjectItem _projectItem;

        public ProjectItemCollection(ProjectItem projectItem)
        {
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            _projectItem = projectItem;
        }

        protected override void ClearItems()
        {
            foreach (var item in this)
            {
                item.Parent = null;
            }

            base.ClearItems();

            _projectItem.TreeNode.Nodes.Clear();
        }

        protected override void InsertItem(int index, ProjectItem item)
        {
            if (item.Parent != null)
                throw new InvalidOperationException();

            item.Parent = _projectItem;

            base.InsertItem(index, item);

            _projectItem.TreeNode.Nodes.Insert(index, item.TreeNode);
        }

        protected override void RemoveItem(int index)
        {
            this[index].Parent = null;

            base.RemoveItem(index);

            _projectItem.TreeNode.Nodes.RemoveAt(index);
        }

        protected override void SetItem(int index, ProjectItem item)
        {
            if (item.Parent != null)
                throw new InvalidOperationException();

            this[index].Parent = null;

            item.Parent = _projectItem;

            base.SetItem(index, item);

            _projectItem.TreeNode.Nodes[index] = item.TreeNode;
        }
    }
}
