using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java.Playwright
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
                return Path.Combine(GetMainSourcePath(), CodeFolders.models.ToString(), page.Name + "Model.java");
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
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateFields(page));
            listOfLines.Add($"");
            listOfLines.AddRange(GenerateAccessors(page));
            listOfLines.AddRange(GenerateExtensionMethods(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GeneratePackage()
        {
            return new List<string>
            {
                $"package {GetPackage()}.{CodeFolders.models};"
            };
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            return new List<string>
            {
                $"public class {page.Name}Model"
            };
        }

        internal List<string> GenerateFields(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                    listOfLines.Add($"private String {control.Name.CamelCase()};");
                else if (control.IsCheckBox() || control.IsRadioButton())
                    listOfLines.Add($"private boolean {control.Name.CamelCase()};");
            }

            return listOfLines;
        }

        internal List<string> GenerateAccessors(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                {
                    listOfLines.Add($"public String get{control.Name}() {{");
                    listOfLines.Add($"return {control.Name.CamelCase()};");
                    listOfLines.Add($"}}");
                    listOfLines.Add($"");
                    listOfLines.Add($"public void set{control.Name}(String value) {{");
                    listOfLines.Add($"this.{control.Name.CamelCase()} = value;");
                    listOfLines.Add($"}}");
                    listOfLines.Add($"");
                }
                else if (control.IsCheckBox() || control.IsRadioButton())
                {
                    listOfLines.Add($"public boolean is{control.Name}() {{");
                    listOfLines.Add($"return {control.Name.CamelCase()};");
                    listOfLines.Add($"}}");
                    listOfLines.Add($"");
                    listOfLines.Add($"public void set{control.Name}(boolean value) {{");
                    listOfLines.Add($"this.{control.Name.CamelCase()} = value;");
                    listOfLines.Add($"}}");
                    listOfLines.Add($"");
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateExtensionMethods(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);
            return GenerateSourceCodeExtensionMethods(filePath, "// region Extensions", "// endregion");
        }
    }
}
