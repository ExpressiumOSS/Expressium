using Expressium.Configurations;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.SolutionGenerators
{
    internal abstract class SolutionGeneratorProject
    {
        protected readonly Configuration configuration;

        internal SolutionGeneratorProject(Configuration configuration)
        {
            this.configuration = configuration;
        }

        internal abstract void GenerateAll();

        protected static void WriteToFile(string destinationFile, string text, Dictionary<string, string> mapOfProperties)
        {
            foreach (var property in mapOfProperties)
                text = text.Replace(property.Key, property.Value);

            var directory = Path.GetDirectoryName(destinationFile);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            using (var fileStream = File.Create(destinationFile))
            {
                using (var streamWriter = new StreamWriter(fileStream))
                    streamWriter.WriteLine(text);
            }

            Console.WriteLine(destinationFile);
        }
    }
}
