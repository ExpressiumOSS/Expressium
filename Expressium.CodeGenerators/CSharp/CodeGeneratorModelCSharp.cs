using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorModelCSharp : CodeGeneratorModel
    {
        internal CodeGeneratorModelCSharp(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, configuration.GetNameSpace(), CodeFolders.Models.ToString(), page.Name + "Model.cs");
            }
            catch
            {
                return null;
            }
        }

        internal override List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateUsings());
            listOfLines.AddRange(GenerateNameSpace());
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateProperties(page));
            listOfLines.AddRange(GenerateExtensions(page));
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
                $"namespace {configuration.GetNameSpace()}.{CodeFolders.Models}"
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

        internal List<string> GenerateExtensions(ObjectRepositoryPage page)
        {
            if (configuration.IncludeExtensions)
            {
                var filePath = GetFilePath(page);
                return GenerateExtensions(filePath, "#region Extensions", "#endregion");
            }

            return new List<string>();
        }
    }
}
