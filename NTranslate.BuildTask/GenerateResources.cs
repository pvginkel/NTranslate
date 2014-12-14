using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace NTranslate.BuildTask
{
    public class GenerateResources : AppDomainIsolatedTask
    {
        public string KeyFile { get; set; }

        [Required]
        public string ProjectDir { get; set; }

        [Required]
        public string TargetDir { get; set; }

        [Required]
        public string AssemblyName { get; set; }

        public override bool Execute()
        {
            var directory = Path.Combine(ProjectDir, "Translations");
            if (!Directory.Exists(directory))
                return true;

            var name = GetAssemblyName(Path.Combine(TargetDir, AssemblyName + ".dll"), false);
            if (name == null)
                name = GetAssemblyName(Path.Combine(TargetDir, AssemblyName + ".exe"), true);

            foreach (string fileName in Directory.GetFiles(directory, "*.ntx"))
            {
                new AssemblyGenerator(fileName, name, TargetDir, KeyFile).Execute();
            }

            return true;
        }

        private AssemblyName GetAssemblyName(string fileName, bool throwOnError)
        {
            if (!throwOnError && !File.Exists(fileName))
                return null;

            return Assembly.LoadFile(fileName).GetName();
        }
    }
}
