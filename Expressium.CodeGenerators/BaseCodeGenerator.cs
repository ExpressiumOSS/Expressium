using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators
{
    public abstract class BaseCodeGenerator
    {
        protected Configuration configuration;
        protected ObjectRepository objectRepository;

        public BaseCodeGenerator(Configuration configuration, ObjectRepository objectRepository)
        {
            this.configuration = configuration;
            this.objectRepository = objectRepository;
        }

        internal abstract void Generate(ObjectRepositoryPage page);
        internal abstract string GenerateAsString(ObjectRepositoryPage page);

        internal static void SaveSourceCode(string filePath, List<string> listOfCodeLines)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            using (var streamWriter = new StreamWriter(filePath))
            {
                foreach (var line in listOfCodeLines)
                    streamWriter.WriteLine(line);
            }

            Console.WriteLine(filePath);
        }

        internal static string GetSourceCodeAsString(List<string> listOfCodeLines)
        {
            return string.Join(Environment.NewLine, listOfCodeLines);
        }

        internal static List<string> FormatSourceCode(List<string> listOfCodeLines)
        {
            var listOfStatements = new List<string>
            {
                "if",
                "else if",
                "else",
                "for",
                "while"
            };

            var indentLevel = 0;
            var indentNextLevel = 0;

            var listOfLines = new List<string>();

            foreach (var codeLine in listOfCodeLines)
            {
                var line = codeLine.Trim();

                if (line.StartsWith("}"))
                    indentLevel--;

                if (string.IsNullOrWhiteSpace(line))
                    listOfLines.Add("");
                else
                    listOfLines.Add(new string(' ', 4 * (indentLevel + indentNextLevel)) + line);

                if (line.EndsWith("{"))
                    indentLevel++;

                if (listOfStatements.Any(s => line.StartsWith(s)))
                    indentNextLevel++;
                else if (indentNextLevel > 0)
                    indentNextLevel--;
                else
                {
                }
            }

            return listOfLines;
        }

        internal static List<string> GenerateSourceCodeExtensionMethods(string filePath, string startLine, string endLine)
        {
            var listOfLines = new List<string>();

            listOfLines.Add(startLine);

            var listOfCodeLines = GetSourceCodeSnippetInFile(filePath, startLine, endLine);
            if (listOfCodeLines != null && listOfCodeLines.Count > 0)
                listOfLines.AddRange(listOfCodeLines);
            else
                listOfLines.Add("");

            listOfLines.Add(endLine);

            return listOfLines;
        }

        internal static List<string> GetSourceCodeSnippetInFile(string filePath, string startLine, string endLine)
        {
            var listOfLines = new List<string>();

            if (File.Exists(filePath))
            {
                var reading = false;

                foreach (var line in File.ReadAllLines(filePath))
                {
                    if (reading && line.Trim() == endLine)
                        reading = false;

                    if (reading)
                        listOfLines.Add(line.Trim());

                    if (line.Trim() == startLine)
                        reading = true;
                }
            }

            return listOfLines;
        }

        internal static bool IsTextInSourceCodeFile(string filePath, string text)
        {
            if (File.Exists(filePath) && File.ReadAllText(filePath).Contains(text))
                return true;

            return false;
        }
    }
}
