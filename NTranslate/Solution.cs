using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            "^Project\\(" + QuotedPattern + "\\) = (" + QuotedPattern + "), (" + QuotedPattern + "), " + QuotedPattern + "$",
            RegexOptions.Compiled
        );

        private bool _disposed;

        public ProjectItem RootNode { get; private set; }

        public Solution(string fileName)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            RootNode = new ProjectItem(fileName, false);
            RootNode.SetProperty<Solution>(this);

            LoadSolution();
        }

        private void LoadSolution()
        {
            var projects = new List<ProjectItem>();

            foreach (string line in File.ReadAllLines(RootNode.FileName))
            {
                var match = ProjectLineRe.Match(line);
                if (match.Success)
                {
                    var project = new Project(
                        Path.Combine(
                            Path.GetDirectoryName(RootNode.FileName),
                            Unquote(match.Groups[2].Value)
                        ),
                        Unquote(match.Groups[1].Value)
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

                _disposed = true;
            }
        }
    }
}
