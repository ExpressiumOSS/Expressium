using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators
{
    public abstract class CodeGeneratorObject
    {
        protected Configuration configuration;
        protected ObjectRepository objectRepository;

        internal CodeGeneratorObject(Configuration configuration, ObjectRepository objectRepository)
        {
            this.configuration = configuration;
            this.objectRepository = objectRepository;
        }

        internal abstract void Generate(ObjectRepositoryPage page);
        internal abstract string GenerateAsString(ObjectRepositoryPage page);

        internal static void SaveListOfLinesAsFile(string filePath, List<string> listOfLines)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var streamWriter = new StreamWriter(filePath))
            {
                foreach (var line in listOfLines)
                    streamWriter.WriteLine(line);
            }

            Console.WriteLine(filePath);
        }

        internal static string GetListOfLinesAsString(List<string> listOfLines)
        {
            var value = string.Empty;
            foreach (var line in listOfLines)
                value += line + "\n";

            return value;
        }

        internal static List<string> GetListOfLinesAsFormatted(List<string> listOfLines)
        {
            var listOfFormattedLines = new List<string>();

            var previousLine = string.Empty;
            var numberOfIndent = 0;
            foreach (var l in listOfLines)
            {
                var line = l.Trim();

                if (line.StartsWith("}"))
                    numberOfIndent--;

                var sIndent = string.Empty;
                for (int i = 0; i < numberOfIndent; i++)
                    sIndent += "    ";

                if (line.StartsWith(": base"))
                    sIndent += "    ";

                if (!line.StartsWith("{"))
                {
                    if (previousLine.StartsWith("if") || previousLine.StartsWith("else if") || previousLine.StartsWith("else") || previousLine.StartsWith("for"))
                        if (!line.StartsWith("if"))
                            sIndent += "    ";

                }

                var formattedLine = sIndent + line;
                listOfFormattedLines.Add(formattedLine.TrimEnd());

                if (line.StartsWith("{"))
                    numberOfIndent++;

                previousLine = line;
            }

            return listOfFormattedLines;
        }

        internal static List<string> GenerateExtensions(string filePath, string startLine, string endLine)
        {
            var listOfLines = new List<string>();

            listOfLines.Add(startLine);

            var listOfExtensionCodeLines = GetExtensionsInFile(filePath, startLine, endLine);
            if (listOfExtensionCodeLines != null && listOfExtensionCodeLines.Count > 0)
                listOfLines.AddRange(listOfExtensionCodeLines);
            else
                listOfLines.Add("");

            listOfLines.Add(endLine);

            return listOfLines;
        }

        internal static List<string> GetExtensionsInFile(string filePath, string startLine, string endLine)
        {
            var listOfLines = new List<string>();

            if (File.Exists(filePath))
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    var reading = false;
                    var line = string.Empty;

                    while ((line = streamReader.ReadLine()) != null)
                    {
                        line = line.Trim();

                        if (reading && line == endLine)
                            reading = false;

                        if (reading)
                        {
                            if (line != "{" && line.EndsWith("{") && !line.StartsWith("//"))
                            {
                                line = line.TrimEnd('{');
                                listOfLines.Add(line.Trim());
                                listOfLines.Add("{");
                            }
                            else
                            {
                                listOfLines.Add(line);
                            }
                        }

                        if (line == startLine)
                            reading = true;
                    }
                }
            }

            return listOfLines;
        }

        internal static bool IsTestFileModified(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (var streamReader = new StreamReader(filePath))
                {
                    string line = string.Empty;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        if (line.Contains("// TODO - Implement"))
                            return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
