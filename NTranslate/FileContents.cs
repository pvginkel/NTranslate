using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows.Forms;
using NTranslate.Dto;

namespace NTranslate
{
    public class FileContents
    {
        public static FileContents Load(ProjectItem projectItem, FileDto translationsFile)
        {
            return Load(Program.ProjectManager.CurrentProject, projectItem, translationsFile);
        }

        public static FileContents Load(Project project, ProjectItem projectItem, FileDto translationsFile)
        {
            if (project == null)
                throw new ArgumentNullException("project");
            if (projectItem == null)
                throw new ArgumentNullException("projectItem");

            var nodes = new List<FileNode>();
            var resourceNodes = new Dictionary<string, NodeDto>();

            if (translationsFile != null)
            {
                foreach (var node in translationsFile.Nodes)
                {
                    resourceNodes.Add(node.Name, node);
                }
            }

            string currentDirectory = Environment.CurrentDirectory;

            try
            {
                var directory = project.Directory;
                Environment.CurrentDirectory = directory;

                using (var reader = new ResXResourceReader(Path.Combine(directory, projectItem.FileName)))
                {
                    reader.UseResXDataNodes = true;

                    foreach (DictionaryEntry entry in reader)
                    {
                        var node = (ResXDataNode)entry.Value;

                        if (node.GetValue((ITypeResolutionService)null) is string)
                            AddNode(nodes, resourceNodes, node);
                    }
                }
            }
            finally
            {
                Environment.CurrentDirectory = currentDirectory;
            }

            return new FileContents(nodes);
        }

        private static void AddNode(List<FileNode> nodes, Dictionary<string, NodeDto> resourceNodes, ResXDataNode entry)
        {
            NodeDto node;
            if (!resourceNodes.TryGetValue(entry.Name, out node))
            {
                node = new NodeDto
                {
                    Name = entry.Name,
                    Source = (string)entry.GetValue((ITypeResolutionService)null)
                };
            }

            nodes.Add(new FileNode(
                node.Name,
                (string)entry.GetValue((ITypeResolutionService)null),
                node.Source,
                node.Text,
                entry.Comment
            ));
        }

        public IList<FileNode> Nodes { get; private set; }

        private FileContents(IList<FileNode> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException("nodes");

            Nodes = new ReadOnlyCollection<FileNode>(nodes);
        }
    }
}
