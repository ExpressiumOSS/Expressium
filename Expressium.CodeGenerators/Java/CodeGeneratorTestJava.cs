using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorTestJava : CodeGeneratorTest
    {
        internal CodeGeneratorTestJava(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetSourceCodeFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, @"src\test\java", TestFolders.UITests.ToString(), page.Name + "Tests.java");
            }
            catch
            {
                return null;
            }
        }

        internal override List<string> GenerateSourceCode(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.AddRange(GenerateImports());
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GeneratedDataVariables(page));
            listOfLines.AddRange(GenerateOneTimeSetUpMethod(page));
            listOfLines.AddRange(GeneratePageTitleTestMethod(page));
            listOfLines.AddRange(GenerateControlTestMethods(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateImports()
        {
            var listOfLines = new List<string>
            {
                $"package {TestFolders.UITests};",
                "",
                "import Bases.Asserts;",
                "import Bases.BaseTest;",
                "import Factories.*;",
                "import Models.*;",
                "import Pages.*;",
                "import org.testng.annotations.BeforeClass;",
                "import org.testng.annotations.Test;",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"public class {page.Name}Tests extends BaseTest"
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
                                    listOfLines.Add($"private String {control.Name.CamelCase()} = \"{CodeGeneratorUtilities.GenerateRandomString(6)}\";");
                                else
                                    listOfLines.Add($"private String {control.Name.CamelCase()} = \"{control.Value}\";");
                            }
                            else if (control.IsCheckBox() || control.IsRadioButton())
                            {
                                if (string.IsNullOrWhiteSpace(control.Value))
                                    listOfLines.Add($"private boolean {control.Name.CamelCase()} = true;");
                                else
                                {
                                    if (control.Value.ToLower() == "true")
                                        listOfLines.Add($"private boolean {control.Name.CamelCase()} = true;");
                                    else
                                        listOfLines.Add($"private boolean {control.Name.CamelCase()} = false;");
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

            listOfLines.Add($"@BeforeClass");
            listOfLines.Add($"public void oneTimeSetUp() throws Exception");
            listOfLines.Add($"{{");

            if (!string.IsNullOrWhiteSpace(configuration.InitialLoginPage) && !configuration.InitialLoginPage.Contains(page.Name))
                listOfLines.Add($"initializeBrowserWithLogin();");
            else
                listOfLines.Add($"initializeBrowser();");
            listOfLines.Add($"");

            listOfLines.Add($"// TODO - Implement potential missing page navigations...");
            listOfLines.Add($"");

            var listOfNavigationLines = GetNavigationMethods(objectRepository, page.Name);
            if (listOfNavigationLines != null)
                listOfLines.AddRange(listOfNavigationLines);

            listOfLines.Add($"{page.Name.CamelCase()} = new {page.Name}(driver);");

            foreach (var control in page.Controls)
            {
                if (control.IsFillFormControl())
                {
                    if (page.Model)
                        listOfLines.Add($"{page.Name.CamelCase()}.set{control.Name}({page.Name.CamelCase()}Model.get{control.Name}());");
                    else
                        listOfLines.Add($"{page.Name.CamelCase()}.set{control.Name}({control.Name.CamelCase()});");
                }
            }

            listOfLines.Add($"}}");
            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GeneratePageTitleTestMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"@Test",
                $"public void validate_Page_Property_Title()",
                $"{{",
                $"Asserts.assertEquals(\"{page.Title}\", {page.Name.CamelCase()}.getTitle(), \"Validating the {page.Name} Title...\");",
                $"}}"
            };

            return listOfLines;
        }

        internal List<string> GenerateControlTestMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsFillFormControl())
                {
                    listOfLines.Add($"");
                    listOfLines.Add($"@Test");
                    listOfLines.Add($"public void validate_Page_Property_{control.Name}() throws Exception");
                    listOfLines.Add($"{{");

                    if (page.Model)
                        listOfLines.Add($"Asserts.assertEquals({page.Name.CamelCase()}Model.get{control.Name}(), {page.Name.CamelCase()}.get{control.Name}(), \"Validating the {page.Name} property {control.Name}...\");");
                    else
                        listOfLines.Add($"Asserts.assertEquals({control.Name.CamelCase()}, {page.Name.CamelCase()}.get{control.Name}(), \"Validating the {page.Name} property {control.Name}...\");");

                    listOfLines.Add($"}}");
                }
            }

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

                        listOfLines.Add($"{page.Name} {page.Name.CamelCase()} = new {page.Name}(driver);");
                        listOfLines.Add($"{page.Name.CamelCase()}.click{control.Name}();");
                        listOfLines.Add($"");

                        return listOfLines;
                    }
                }
            }

            return null;
        }
    }
}
