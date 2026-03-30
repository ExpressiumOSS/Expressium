using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java.Selenium
{
    internal class CodeGeneratorFactory : CodeGeneratorObject
    {
        internal CodeGeneratorFactory(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal void Generate(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);

            if (IsSourceCodeModified(filePath))
                return;

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
                return Path.Combine(GetTestSourcePath(), TestFolders.factories.ToString(), page.Name + "ModelFactory.java");
            }
            catch
            {
                return null;
            }
        }

        internal List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GeneratePackage());
            listOfLines.Add($"");
            listOfLines.AddRange(GenerateImports());
            listOfLines.Add($"");
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateDefaultMethod(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GeneratePackage()
        {
            return new List<string>
            {
                $"package {GetPackage()}.{TestFolders.factories};"
            };
        }

        internal List<string> GenerateImports()
        {
            return new List<string>
            {
                $"import {GetPackage()}.{CodeFolders.models}.*;"
            };
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            return new List<string>
            {
                $"public class {page.Name}ModelFactory"
            };
        }

        internal List<string> GenerateDefaultMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"public static {page.Name}Model getDefault() {{",
                $"{page.Name}Model model = new {page.Name}Model();",
                $"",
                $"// TODO - Implement potential missing factory property...",
                $""
            };

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
            }

            listOfLines.Add($"");
            listOfLines.Add($"return model;");
            listOfLines.Add($"}}");

            return listOfLines;
        }
    }
}
