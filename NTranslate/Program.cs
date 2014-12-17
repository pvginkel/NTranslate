using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace NTranslate
{
    internal static class Program
    {
        public static MainForm MainForm { get; private set; }
        public static SolutionManager SolutionManager { get; private set; }
        public static DocumentManager DocumentManager { get; private set; }
        public static SpellCheck.SpellCheck SpellCheck { get; private set; }

        public static RegistryKey BaseKey
        {
            get { return Registry.CurrentUser.CreateSubKey("Software\\NTranslate"); }
        }

        public static string BasePath { get; private set; }

        [STAThread]
        public static void Main(string[] args)
        {
            BasePath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "NTranslate"
            );

            Directory.CreateDirectory(BasePath);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            MouseWheelMessageFilter.Install();

            SpellCheck = new SpellCheck.SpellCheck();
            SolutionManager = new SolutionManager();
            MainForm = new MainForm(args);
            DocumentManager = new DocumentManager();

            Application.Run(MainForm);
        }
    }
}
