using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorModelJava : CodeGeneratorModel
    {
        internal CodeGeneratorModelJava(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, @"src\main\java", CodeFolders.Models.ToString(), page.Name + "Model.java");
            }
            catch
            {
                return null;
            }
        }

        internal override List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateImports(page));
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateAttributes(page));
            listOfLines.AddRange(GenerateMethods(page));
            listOfLines.AddRange(GenerateExtensions(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateImports(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"package Models;",
                $"",
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

        internal List<string> GenerateAttributes(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                {
                    listOfLines.Add($"private String {control.Name.CamelCase()};");
                }
                else if (control.IsCheckBox() || control.IsRadioButton())
                {
                    listOfLines.Add($"private boolean {control.Name.CamelCase()};");
                }
                else
                {
                }
            }

            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GenerateMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                {
                    listOfLines.AddRange(new List<string>
                    {
                        $"public String get{control.Name}()",
                        $"{{",
                        $"return {control.Name.CamelCase()};",
                        $"}}",
                        $"",
                        $"public void set{control.Name}(String {control.Name.CamelCase()})",
                        $"{{",
                        $"this.{control.Name.CamelCase()} = {control.Name.CamelCase()};",
                        $"}}",
                        $""
                    });
                }
                else if (control.IsCheckBox() || control.IsRadioButton())
                {
                    listOfLines.AddRange(new List<string>
                    {
                        $"public boolean get{control.Name}()",
                        $"{{",
                        $"return {control.Name.CamelCase()};",
                        $"}}",
                        $"",
                        $"public void set{control.Name}(boolean {control.Name.CamelCase()})",
                        $"{{",
                        $"this.{control.Name.CamelCase()} = {control.Name.CamelCase()};",
                        $"}}",
                        $""
                    });
                }
                else
                {
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateExtensions(ObjectRepositoryPage page)
        {
            if (configuration.IncludeExtensions)
            {
                var filePath = GetFilePath(page);
                return GenerateExtensions(filePath, "// region Extensions", "// endregion");
            }

            return new List<string>();
        }
    }
}
