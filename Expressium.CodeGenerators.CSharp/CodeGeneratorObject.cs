using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System;
using System.Linq;
using System.IO;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorObject
    {
        protected Configuration configuration;
        protected ObjectRepository objectRepository;

        internal CodeGeneratorObject(Configuration configuration, ObjectRepository objectRepository)
        {
            this.configuration = configuration;
            this.objectRepository = objectRepository;
        }

        internal static bool IsSourceCodeModified(string filePath)
        {
            if (File.Exists(filePath) && !File.ReadAllText(filePath).Contains("// TODO - Implement"))
                return true;

            return false;
        }

        internal static void SaveSourceCode(string filePath, List<string> listOfCodeLines)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllLines(filePath, listOfCodeLines);

            Console.WriteLine(filePath);
        }

        internal static string GetSourceCodeAsString(List<string> listOfCodeLines)
        {
            return string.Join(Environment.NewLine, listOfCodeLines);
        }

        internal static List<string> GetSourceCodeAsFormatted(List<string> listOfCodeLines)
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

        internal bool IsCodingFlavourSpecflow()
        {
            if (configuration.CodeGenerator.CodingFlavour == CodingFlavours.Specflow.ToString())
                return true;

            return false;
        }

        internal bool IsCodingFlavourReqnroll()
        {
            if (configuration.CodeGenerator.CodingFlavour == CodingFlavours.Reqnroll.ToString())
                return true;

            return false;
        }

        internal bool IsCodingStylePageFactory()
        {
            if (configuration.CodeGenerator.CodingStyle == CodingStyles.PageFactory.ToString())
                return true;

            return false;
        }

        internal bool IsCodingStyleByLocators()
        {
            if (configuration.CodeGenerator.CodingStyle == CodingStyles.ByLocators.ToString())
                return true;

            return false;
        }

        internal bool IsCodingStyleByControls()
        {
            if (configuration.CodeGenerator.CodingStyle == CodingStyles.ByControls.ToString())
                return true;

            return false;
        }

        internal string GetNameSpace()
        {
            return $"{configuration.Company}.{configuration.Project}.Web.API";
        }
    }
}
