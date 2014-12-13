using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate.App
{
    public class ProjectManager
    {
        private Project _currentProject;

        public event EventHandler CurrentProjectChanged;

        protected virtual void OnCurrentProjectChanged(EventArgs e)
        {
            var handler = CurrentProjectChanged;
            if (handler != null)
                handler(this, e);
        }

        public Project CurrentProject
        {
            get { return _currentProject; }
            private set
            {
                if (_currentProject != value)
                {
                    _currentProject = value;
                    OnCurrentProjectChanged(EventArgs.Empty);
                }
            }
        }

        public void OpenProject(string fileName)
        {
            if (_currentProject != null && !CloseProject())
                return;

            CurrentProject = new Project(fileName);
        }

        public bool CloseProject()
        {
            throw new NotImplementedException();
        }
    }
}
