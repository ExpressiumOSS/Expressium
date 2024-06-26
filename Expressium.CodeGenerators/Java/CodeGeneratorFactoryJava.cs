using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorFactoryJava : CodeGeneratorObject, ICodeGeneratorFactory
    {
        internal CodeGeneratorFactoryJava(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        public void Generate(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);

            if (IsSourceCodeModified(filePath))
                return;

            var sourceCode = GenerateSourceCode(page);
            var listOfLines = GetSourceCodeAsFormatted(sourceCode);
            SaveSourceCode(filePath, listOfLines);
        }

        public string GeneratePreview(ObjectRepositoryPage page)
        {
            var sourceCode = GenerateSourceCode(page);
            var listOfLines = GetSourceCodeAsFormatted(sourceCode);
            return GetSourceCodeAsString(listOfLines);
        }

        internal string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, @"src\test\java", TestFolders.Factories.ToString(), page.Name + "ModelFactory.java");
            }
            catch
            {
                return null;
            }
        }

        internal List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateImports(page));
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateDefaultMethod(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateImports(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"package Factories;",
                $"",
                $"import Models.{page.Name}Model;",
                $""
            };

            return listOfLines;
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"public class {page.Name}ModelFactory"
            };

            return listOfLines;
        }

        internal List<string> GenerateDefaultMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public static {page.Name}Model Default()");
            listOfLines.Add($"{{");
            listOfLines.Add($"{page.Name}Model model = new {page.Name}Model();");
            listOfLines.Add($"");
            listOfLines.Add($"// TODO - Implement potential missing factory property...");
            listOfLines.Add($"");

            foreach (var control in page.Controls)
            {
                if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                {
                    string value = control.Value;
                    if (string.IsNullOrWhiteSpace(value))
                        value = CodeGeneratorUtilities.GenerateRandomString(6);

                    listOfLines.Add($"model.set{control.Name}(\"{value}\");");
                }
                else if (control.IsCheckBox() || control.IsRadioButton())
                {
                    if (control.Value != null && control.Value.ToLower() == "true")
                        listOfLines.Add($"model.set{control.Name}(true);");
                    else
                        listOfLines.Add($"model.set{control.Name}(false);");
                }
                else
                {
                }
            }

            listOfLines.Add($"");
            listOfLines.Add($"return model;");
            listOfLines.Add($"}}");

            return listOfLines;
        }
    }
}
