using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            base.ClearItems();

            _projectItem.TreeNode.Nodes.Clear();
        }

        protected override void InsertItem(int index, ProjectItem item)
        {
            base.InsertItem(index, item);

            _projectItem.TreeNode.Nodes.Insert(index, item.TreeNode);
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            _projectItem.TreeNode.Nodes.RemoveAt(index);
        }

        protected override void SetItem(int index, ProjectItem item)
        {
            base.SetItem(index, item);

            _projectItem.TreeNode.Nodes[index] = item.TreeNode;
        }
    }
}
