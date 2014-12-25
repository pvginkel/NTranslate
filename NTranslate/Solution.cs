using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NTranslate
{
    public class Solution : IDisposable
    {
        private const string QuotedPattern = "\"[^\"]*\"";
        private static readonly Regex ProjectLineRe = new Regex(
            "^Project\\((" + QuotedPattern + ")\\) = (" + QuotedPattern + "), (" + QuotedPattern + "), " + QuotedPattern + "$",
            RegexOptions.Compiled
        );

        private bool _disposed;

        public ProjectItem RootNode { get; private set; }
        public TranslationDictionary Dictionary { get; private set; }

        public Solution(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            RootNode = new ProjectItem(fileName, false);
            RootNode.SetProperty<Solution>(this);

            Dictionary = new TranslationDictionary();

            LoadSolution();
            Program.MainForm.LanguageChanged += MainForm_LanguageChanged;
        }

        void MainForm_LanguageChanged(object sender, EventArgs e)
        {
            Dictionary = new TranslationDictionary();
        }

        private void LoadSolution()
        {
            var projects = new List<ProjectItem>();

            foreach (string line in File.ReadAllLines(RootNode.FileName))
            {
                var match = ProjectLineRe.Match(line);
                if (match.Success)
                {
                    string guid = match.Groups[1].Value;

                    // Ignore folders.

                    if (guid.ToUpperInvariant().Contains("2150E333-8FDC-42A3-9474-1A3956D46DE8"))
                        continue;

                    var project = new Project(
                        this,
                        Path.Combine(
                            Path.GetDirectoryName(RootNode.FileName),
                            Unquote(match.Groups[3].Value)
                        ),
                        Unquote(match.Groups[2].Value)
                    );

                    projects.Add(project.RootNode);
                }
            }

            projects.Sort((a, b) => String.Compare(a.Name, b.Name, StringComparison.InvariantCulture));

            RootNode.Children.AddRange(projects);
        }

        private string Unquote(string value)
        {
            if (String.IsNullOrEmpty(value) || value.Length < 2 || value[0] != '"' || value[value.Length - 1] != '"')
                throw new ArgumentException("Unexpected quoted value");

            string result = value.Substring(1, value.Length - 2);

            Debug.Assert(!result.Contains('"'));

            return result;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                foreach (var projectItem in RootNode.Children)
                {
                    projectItem.GetProperty<Project>().Dispose();
                }

                Program.MainForm.LanguageChanged -= MainForm_LanguageChanged;

                _disposed = true;
            }
        }
    }
}
