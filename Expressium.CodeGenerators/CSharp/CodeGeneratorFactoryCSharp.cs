using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorFactoryCSharp : CodeGeneratorFactory
    {
        internal CodeGeneratorFactoryCSharp(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, configuration.GetNameSpace() + ".Tests", TestFolders.Factories.ToString(), page.Name + "ModelFactory.cs");
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
            listOfLines.AddRange(GenerateDefaultMethod(page));
            listOfLines.Add($"}}");
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal override bool IsFileModified(string filePath)
        {
            if (IsSourceCodeTextInFile(filePath, "// TODO - Implement"))
                return true;

            return false;
        }

        internal List<string> GenerateUsings()
        {
            var listOfLines = new List<string>
            {
                $"using {configuration.GetNameSpace()}.Models;",
                $"using System;",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateNameSpace()
        {
            var listOfLines = new List<string>
            {
                $"namespace {configuration.GetNameSpace()}.Tests.{TestFolders.Factories}"
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
            var listOfLines = new List<string>
            {
                $"public static {page.Name}Model Default()",
                $"{{",
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

                    listOfLines.Add($"model.{control.Name} = \"{value}\";");
                }
                else if (control.IsCheckBox() || control.IsRadioButton())
                {
                    if (control.Value != null && control.Value.ToLower() == "true")
                        listOfLines.Add($"model.{control.Name} = true;");
                    else
                        listOfLines.Add($"model.{control.Name} = false;");
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
