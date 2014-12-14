using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Xml;

namespace NTranslate.BuildTask
{
    internal class AssemblyGenerator
    {
        private readonly string _fileName;
        private readonly AssemblyName _assemblyName;
        private readonly string _targetDir;
        private readonly string _keyFile;

        public AssemblyGenerator(string fileName, AssemblyName assemblyName, string targetDir, string keyFile)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");
            if (assemblyName == null)
                throw new ArgumentNullException("assemblyName");
            if (targetDir == null)
                throw new ArgumentNullException("targetDir");

            _fileName = fileName;
            _assemblyName = assemblyName;
            _targetDir = targetDir;
            _keyFile = keyFile;
        }

        public void Execute()
        {
            BuildAssembly(ReadResource());
        }

        private void BuildAssembly(IList<FileDto> files)
        {
            var cultureName = Path.GetFileNameWithoutExtension(_fileName);

            var name = new AssemblyName(_assemblyName.Name + ".resources")
            {
                Version = _assemblyName.Version,
                CultureInfo = CultureInfo.GetCultureInfo(cultureName)
            };

            using (var stream = File.OpenRead(_keyFile))
            {
                name.KeyPair = new StrongNameKeyPair(stream);
            }

            var targetDir = Path.Combine(_targetDir, cultureName);

            Directory.CreateDirectory(targetDir);

            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(
                name,
                AssemblyBuilderAccess.Save,
                targetDir
            );
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(
                name.Name + ".dll",
                name.Name + ".dll"
            );

            foreach (var file in files)
            {
                if (!file.Name.EndsWith(".resx", StringComparison.OrdinalIgnoreCase))
                    continue;

                string resourceName = file.Name.Substring(0, file.Name.Length - 5) + "." + cultureName + ".resources";

                var writer = moduleBuilder.DefineResource(
                    resourceName,
                    null
                );

                foreach (var node in file.Nodes)
                {
                    writer.AddResource(node.Name, node.Text);
                }
            }

            assemblyBuilder.Save(name.Name + ".dll");
        }

        private IList<FileDto> ReadResource()
        {
            using (var stream = File.OpenRead(_fileName))
            using (var reader = XmlReader.Create(stream))
            {
                return FileDto.Load(reader);
            }
        }
    }
}
