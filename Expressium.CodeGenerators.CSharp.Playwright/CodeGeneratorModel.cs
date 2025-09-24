using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.CSharp.Playwright
{
    internal class CodeGeneratorModel : CodeGeneratorObject
    {
        internal CodeGeneratorModel(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal void Generate(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);
            var sourceCode = GenerateSourceCode(page);
            var listOfLines = GetSourceCodeAsFormatted(sourceCode);
            SaveSourceCode(filePath, listOfLines);
        }

        internal string GeneratePreview(ObjectRepositoryPage page)
        {
            var sourceCode = GenerateSourceCode(page);
            var listOfLines = GetSourceCodeAsFormatted(sourceCode);
            return GetSourceCodeAsString(listOfLines);
        }

        internal string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, GetNameSpace(), CodeFolders.Models.ToString(), page.Name + "Model.cs");
            }
            catch
            {
                return null;
            }
        }

        internal List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateUsings());
            listOfLines.AddRange(GenerateNameSpace());
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateProperties(page));
            listOfLines.AddRange(GenerateExtensionMethods(page));
            listOfLines.Add($"}}");
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateUsings()
        {
            var listOfLines = new List<string>
            {
                "using System;",
                "using System.Collections.Generic;",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateNameSpace()
        {
            var listOfLines = new List<string>
            {
                $"namespace {GetNameSpace()}.{CodeFolders.Models}"
            };

            return listOfLines;
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"public class {page.Name}Model"
            };

            return listOfLines;
        }

        internal List<string> GenerateProperties(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                {
                    listOfLines.Add($"public string {control.Name} {{ get; set; }}");
                }
                else if (control.IsCheckBox() || control.IsRadioButton())
                {
                    listOfLines.Add($"public bool {control.Name} {{ get; set; }}");
                }
                else
                {
                }
            }

            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GenerateExtensionMethods(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);
            return GenerateSourceCodeExtensionMethods(filePath, "#region Extensions", "#endregion");
        }
    }
}
