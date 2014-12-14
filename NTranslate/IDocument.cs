using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NTranslate
{
    public interface IDocument
    {
        ProjectItem ProjectItem { get; }

        bool IsDirty { get; }

        void Show();

        void Close();

        void Save();
    }
}
