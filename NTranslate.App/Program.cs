using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NTranslate.App
{
    internal static class Program
    {
        public static MainForm MainForm { get; private set; }
        public static ProjectManager ProjectManager { get; private set; }
        public static DocumentManager DocumentManager { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            ProjectManager = new ProjectManager();
            MainForm = new MainForm(args);
            DocumentManager = new DocumentManager();

            Application.Run(MainForm);
        }
    }
}
