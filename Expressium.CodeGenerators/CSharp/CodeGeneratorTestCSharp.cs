using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorTestCSharp : CodeGeneratorTest
    {
        internal CodeGeneratorTestCSharp(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetSourceCodeFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, configuration.GetNameSpace() + ".Tests", TestFolders.UITests.ToString(), page.Name + "Tests.cs");
            }
            catch
            {
                return null;
            }
        }

        internal override List<string> GenerateSourceCode(ObjectRepositoryPage page)
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
                listOfLines.Add($"using {configuration.GetNameSpace()}.Models;");

            if (objectRepository.Pages.Count > 0)
                listOfLines.Add($"using {configuration.GetNameSpace()}.Pages;");

            if (page.Model)
                listOfLines.Add($"using {configuration.GetNameSpace()}.Tests.Factories;");

            listOfLines.Add("using System;");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateNameSpace()
        {
            var listOfLines = new List<string>
            {
                $"namespace {configuration.GetNameSpace()}.Tests.{TestFolders.UITests}"
            };

            return listOfLines;
        }

        internal List<string> GenerateTagProperties(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"[Property(\"Id\",\"TC100??\")]",
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
                            if (control.IsTextBox() || control.IsComboBox())
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

            if (!string.IsNullOrWhiteSpace(configuration.InitialLoginPage) && !configuration.InitialLoginPage.Contains(page.Name))
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
                    listOfLines.Add($"public void Validate_Page_Number_Of_{control.Name}()");
                    listOfLines.Add($"{{");
                    listOfLines.Add($"Asserts.GreaterThan({page.Name.CamelCase()}.GetNumberOf{control.Name}(), -1, \"Validating the {page.Name} {control.Name} number of table lines...\");");
                    listOfLines.Add($"}}");

                    if (!string.IsNullOrWhiteSpace(control.Value))
                    {
                        var listOfHeaders = control.Value.Split(';');
                        var mapOfUsedHeaders = new Dictionary<string, string>();

                        for (int i = 0; i < listOfHeaders.Count(); i++)
                        {
                            var header = listOfHeaders[i].Trim();
                            header = header.PascalCase();

                            if (mapOfUsedHeaders.ContainsKey(header))
                                continue;

                            mapOfUsedHeaders.Add(header, header);

                            if (!string.IsNullOrWhiteSpace(header))
                            {
                                listOfLines.Add($"");
                                listOfLines.Add($"[Test]");
                                listOfLines.Add($"public void Validate_Page_{control.Name}_{header}()");
                                listOfLines.Add($"{{");
                                listOfLines.Add($"Asserts.IsNotNull({page.Name.CamelCase()}.Get{control.Name}{header}Text(1), \"Validating the {page.Name} {control.Name} table property {header}...\");");
                                listOfLines.Add($"}}");
                            }
                        }
                    }
                    else
                    {
                        listOfLines.Add($"");
                        listOfLines.Add($"[Test]");
                        listOfLines.Add($"public void Validate_Page_{control.Name}_Text()");
                        listOfLines.Add($"{{");
                        listOfLines.Add($"Asserts.IsNotNull({page.Name.CamelCase()}.Get{control.Name}Text(1, 3), \"Validating the {page.Name} {control.Name} table property Text...\");");
                        listOfLines.Add($"}}");
                    }
                }
            }

            return listOfLines;
        }
    }
}
