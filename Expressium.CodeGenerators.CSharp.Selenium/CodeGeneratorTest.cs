using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.CSharp.Selenium
{
    internal class CodeGeneratorTest : CodeGeneratorObject
    {
        internal CodeGeneratorTest(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
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
                return Path.Combine(configuration.SolutionPath, GetNameSpace() + ".Tests", TestFolders.UITests.ToString(), page.Name + "Tests.cs");
            }
            catch
            {
                return null;
            }
        }

        internal List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateUsings(page));
            listOfLines.AddRange(GenerateNameSpace());
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateTagProperties(page));
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GeneratedDataVariables(page));
            listOfLines.AddRange(GenerateOneTimeSetUpMethod(page));
            listOfLines.AddRange(GenerateTitleTestMethod(page));
            listOfLines.AddRange(GenerateFillFormTestMethods(page));
            listOfLines.AddRange(GenerateTableTestMethods(page));
            listOfLines.Add($"}}");
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateUsings(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("using NUnit.Framework;");

            if (page.Model)
                listOfLines.Add($"using {GetNameSpace()}.Models;");

            if (objectRepository.Pages.Count > 0)
                listOfLines.Add($"using {GetNameSpace()}.Pages;");

            if (page.Model)
                listOfLines.Add($"using {GetNameSpace()}.Tests.Factories;");

            listOfLines.Add("using System;");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateNameSpace()
        {
            var listOfLines = new List<string>
            {
                $"namespace {GetNameSpace()}.Tests.{TestFolders.UITests}"
            };

            return listOfLines;
        }

        internal List<string> GenerateTagProperties(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"[Category(\"{TestFolders.UITests}\")]",
                $"[Description(\"Validate Navigation & Content of {page.Name}\")]"
            };

            return listOfLines;
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
               $"public class {page.Name}Tests : BaseTest"
            };

            return listOfLines;
        }

        internal List<string> GeneratedDataVariables(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"private {page.Name} {page.Name.CamelCase()};");

            if (page.Model)
            {
                listOfLines.Add($"private {page.Name}Model {page.Name.CamelCase()}Model = {page.Name}ModelFactory.Default();");
            }
            else
            {
                if (page.HasFillFormControls())
                {
                    listOfLines.Add($"");

                    foreach (var control in page.Controls)
                    {
                        if (control.IsFillFormControl())
                        {
                            if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
                            {
                                if (string.IsNullOrWhiteSpace(control.Value))
                                    listOfLines.Add($"private string {control.Name.CamelCase()} = \"{CodeGeneratorUtilities.GenerateRandomString(6)}\";");
                                else
                                    listOfLines.Add($"private string {control.Name.CamelCase()} = \"{control.Value}\";");
                            }
                            else if (control.IsCheckBox() || control.IsRadioButton())
                            {
                                if (string.IsNullOrWhiteSpace(control.Value))
                                    listOfLines.Add($"private bool {control.Name.CamelCase()} = true;");
                                else
                                {
                                    if (control.Value.ToLower() == "true")
                                        listOfLines.Add($"private bool {control.Name.CamelCase()} = true;");
                                    else
                                        listOfLines.Add($"private bool {control.Name.CamelCase()} = false;");
                                }
                            }
                            else
                            {
                            }
                        }
                    }
                }
            }

            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GenerateOneTimeSetUpMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"[OneTimeSetUp]");
            listOfLines.Add($"public void OneTimeSetUp()");
            listOfLines.Add($"{{");

            if (!string.IsNullOrWhiteSpace(configuration.CodeGenerator.InitialLoginPage) && !configuration.CodeGenerator.InitialLoginPage.Contains(page.Name))
                listOfLines.Add($"InitializeBrowserWithLogin();");
            else
                listOfLines.Add($"InitializeBrowser();");
            listOfLines.Add($"");

            listOfLines.Add($"// TODO - Implement potential missing page navigations...");
            listOfLines.Add($"");

            var listOfNavigationLines = GetNavigationMethods(objectRepository, page.Name);
            if (listOfNavigationLines != null)
                listOfLines.AddRange(listOfNavigationLines);

            listOfLines.Add($"{page.Name.CamelCase()} = new {page.Name}(logger, driver);");

            if (page.Model)
            {
                listOfLines.Add($"{page.Name.CamelCase()}.FillForm({page.Name.CamelCase()}Model);");
            }
            else
            {
                foreach (var control in page.Controls)
                {
                    if (control.IsFillFormControl())
                        listOfLines.Add($"{page.Name.CamelCase()}.Set{control.Name}({control.Name.CamelCase()});");
                }
            }

            listOfLines.Add($"}}");
            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GetNavigationMethods(ObjectRepository objectRepository, string name)
        {
            return GetNavigationMethods(objectRepository, name, name);
        }

        internal List<string> GetNavigationMethods(ObjectRepository objectRepository, string origin, string name)
        {
            foreach (var page in objectRepository.Pages)
            {
                if (page.Name == origin)
                    continue;

                if (page.Name == name)
                    continue;

                foreach (var control in page.Controls)
                {
                    if (control.Target == name)
                    {
                        var listOfLines = new List<string>();

                        var listOfParentLines = GetNavigationMethods(objectRepository, origin, page.Name);
                        if (listOfParentLines != null)
                            listOfLines.AddRange(listOfParentLines);

                        listOfLines.Add($"var {page.Name.CamelCase()} = new {page.Name}(logger, driver);");
                        listOfLines.Add($"{page.Name.CamelCase()}.Click{control.Name}();");
                        listOfLines.Add($"");

                        return listOfLines;
                    }
                }
            }

            return null;
        }

        internal List<string> GenerateTitleTestMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"[Test]",
                $"public void Validate_Page_Title()",
                $"{{",
                $"Asserts.EqualTo({page.Name.CamelCase()}.GetTitle(), \"{page.Title}\", \"Validating the {page.Name} Title...\");",
                $"}}"
            };

            return listOfLines;
        }

        internal List<string> GenerateFillFormTestMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsFillFormControl())
                {
                    listOfLines.Add($"");
                    listOfLines.Add($"[Test]");
                    listOfLines.Add($"public void Validate_Page_Property_{control.Name}()");
                    listOfLines.Add($"{{");

                    if (page.Model)
                        listOfLines.Add($"Asserts.EqualTo({page.Name.CamelCase()}.Get{control.Name}(), {page.Name.CamelCase()}Model.{control.Name}, \"Validating the {page.Name} property {control.Name}...\");");
                    else
                        listOfLines.Add($"Asserts.EqualTo({page.Name.CamelCase()}.Get{control.Name}(), {control.Name.CamelCase()}, \"Validating the {page.Name} property {control.Name}...\");");

                    listOfLines.Add($"}}");
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateTableTestMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                {
                    listOfLines.Add($"");
                    listOfLines.Add($"[Test]");
                    listOfLines.Add($"public void Validate_Page_{control.Name}_Number_Of_Rows()");
                    listOfLines.Add($"{{");
                    listOfLines.Add($"Asserts.GreaterThan({page.Name.CamelCase()}.{control.Name}.GetNumberOfRows(), -1, \"Validating the {page.Name} {control.Name} number of rows...\");");
                    listOfLines.Add($"}}");

                    listOfLines.Add($"");
                    listOfLines.Add($"[Test]");
                    listOfLines.Add($"public void Validate_Page_{control.Name}_Number_Of_Columns()");
                    listOfLines.Add($"{{");
                    listOfLines.Add($"Asserts.GreaterThan({page.Name.CamelCase()}.{control.Name}.GetNumberOfColumns(), -1, \"Validating the {page.Name} {control.Name} number of columns...\");");
                    listOfLines.Add($"}}");

                    var numberOfHeaders = 0;
                    if (!string.IsNullOrWhiteSpace(control.Value))
                        numberOfHeaders = control.Value.Split(';').Length;

                    if (numberOfHeaders > 1)
                    {
                        var listOfHeaders = control.Value.Split(';');
                        foreach (var header in listOfHeaders)
                        {
                            var name = header.Trim();

                            if (CodeGeneratorUtilities.IsValidClassName(name))
                            {
                                listOfLines.Add($"");
                                listOfLines.Add($"[Test]");
                                listOfLines.Add($"public void Validate_Page_{control.Name}_Cell_Text_{name}()");
                                listOfLines.Add($"{{");
                                listOfLines.Add($"Asserts.IsNotNull({page.Name.CamelCase()}.{control.Name}.GetCellText(1, \"{name}\"), \"Validating the {page.Name} {control.Name} table cell Text {name}...\");");
                                listOfLines.Add($"}}");
                            }
                        }
                    }
                    else
                    {
                        listOfLines.Add($"");
                        listOfLines.Add($"[Test]");
                        listOfLines.Add($"public void Validate_Page_{control.Name}_Cell_Text()");
                        listOfLines.Add($"{{");
                        listOfLines.Add($"Asserts.IsNotNull({page.Name.CamelCase()}.{control.Name}.GetCellText(1, 2), \"Validating the {page.Name} {control.Name} table cell Text...\");");
                        listOfLines.Add($"}}");
                    }
                }
            }

            return listOfLines;
        }
    }
}
