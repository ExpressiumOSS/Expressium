using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java.Selenium
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
                return Path.Combine(GetTestSourcePath(), TestFolders.uitests.ToString(), page.Name + "Tests.java");
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
            listOfLines.AddRange(GenerateImports(page));
            listOfLines.Add($"");
            listOfLines.AddRange(GenerateTagProperties(page));
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GeneratedDataVariables(page));
            listOfLines.AddRange(GenerateSetUpMethod(page));
            listOfLines.AddRange(GenerateTitleTestMethod(page));
            listOfLines.AddRange(GenerateFillFormTestMethods(page));
            listOfLines.AddRange(GenerateTableTestMethods(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GeneratePackage()
        {
            return new List<string>
            {
                $"package {GetPackage()}.{TestFolders.uitests};"
            };
        }

        internal List<string> GenerateImports(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("import org.junit.jupiter.api.*;");
            listOfLines.Add("import static org.junit.jupiter.api.TestInstance.Lifecycle.PER_CLASS;");

            if (page.Model)
                listOfLines.Add($"import {GetPackage()}.{CodeFolders.models}.*;");

            if (objectRepository.Pages.Count > 0)
                listOfLines.Add($"import {GetPackage()}.{CodeFolders.pages}.*;");

            if (page.Model)
                listOfLines.Add($"import {GetPackage()}.factories.*;");

            return listOfLines;
        }

        internal List<string> GenerateTagProperties(ObjectRepositoryPage page)
        {
            return new List<string>
            {
                $"@TestInstance(PER_CLASS)",
                $"@Tag(\"{TestFolders.uitests}\")",
                $"@DisplayName(\"Validate Navigation & Content of {page.Name}\")"
            };
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            return new List<string>
            {
                $"public class {page.Name}Tests extends BaseTest"
            };
        }

        internal List<string> GeneratedDataVariables(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"private {page.Name} {page.Name.CamelCase()};");

            if (page.Model)
            {
                listOfLines.Add($"private {page.Name}Model {page.Name.CamelCase()}Model = {page.Name}ModelFactory.getDefault();");
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
                        }
                    }
                }
            }

            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GenerateSetUpMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"@BeforeAll");
            listOfLines.Add($"public void setUp() {{");

            if (!string.IsNullOrWhiteSpace(configuration.CodeGenerator.InitialLoginPage) && !configuration.CodeGenerator.InitialLoginPage.Contains(page.Name))
                listOfLines.Add($"initializeBrowserWithLogin();");
            else
                listOfLines.Add($"initializeBrowser();");
            listOfLines.Add($"");

            listOfLines.Add($"// TODO - Implement potential missing page navigations...");
            listOfLines.Add($"");

            var listOfNavigationLines = GetNavigationMethods(objectRepository, page.Name);
            if (listOfNavigationLines != null)
                listOfLines.AddRange(listOfNavigationLines);

            listOfLines.Add($"{page.Name.CamelCase()} = new {page.Name}(logger, driver);");

            if (page.Model)
            {
                listOfLines.Add($"{page.Name.CamelCase()}.fillForm({page.Name.CamelCase()}Model);");
            }
            else
            {
                foreach (var control in page.Controls)
                {
                    if (control.IsFillFormControl())
                        listOfLines.Add($"{page.Name.CamelCase()}.set{control.Name}({control.Name.CamelCase()});");
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
                        listOfLines.Add($"{page.Name.CamelCase()}.click{control.Name}();");
                        listOfLines.Add($"");

                        return listOfLines;
                    }
                }
            }

            return null;
        }

        internal List<string> GenerateTitleTestMethod(ObjectRepositoryPage page)
        {
            return new List<string>
            {
                $"@Test",
                $"public void validate_Page_Title() {{",
                $"asserts.equalTo({page.Name.CamelCase()}.getTitle(), \"{page.Title}\", \"Validating the {page.Name} Title...\");",
                $"}}"
            };
        }

        internal List<string> GenerateFillFormTestMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
            {
                if (control.IsFillFormControl())
                {
                    listOfLines.Add($"");
                    listOfLines.Add($"@Test");
                    listOfLines.Add($"public void validate_Page_Property_{control.Name}() {{");

                    if (page.Model)
                    {
                        if (control.IsCheckBox() || control.IsRadioButton())
                            listOfLines.Add($"asserts.equalTo({page.Name.CamelCase()}.is{control.Name}(), {page.Name.CamelCase()}Model.is{control.Name}(), \"Validating the {page.Name} property {control.Name}...\");");
                        else
                            listOfLines.Add($"asserts.equalTo({page.Name.CamelCase()}.get{control.Name}(), {page.Name.CamelCase()}Model.get{control.Name}(), \"Validating the {page.Name} property {control.Name}...\");");
                    }
                    else
                    {
                        if (control.IsCheckBox() || control.IsRadioButton())
                            listOfLines.Add($"asserts.equalTo({page.Name.CamelCase()}.is{control.Name}(), {control.Name.CamelCase()}, \"Validating the {page.Name} property {control.Name}...\");");
                        else
                            listOfLines.Add($"asserts.equalTo({page.Name.CamelCase()}.get{control.Name}(), {control.Name.CamelCase()}, \"Validating the {page.Name} property {control.Name}...\");");
                    }

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
                    listOfLines.Add($"@Test");
                    listOfLines.Add($"public void validate_Page_{control.Name}_Number_Of_Rows() {{");
                    listOfLines.Add($"asserts.greaterThan({page.Name.CamelCase()}.{control.Name.CamelCase()}.getNumberOfRows(), -1, \"Validating the {page.Name} {control.Name} number of rows...\");");
                    listOfLines.Add($"}}");

                    listOfLines.Add($"");
                    listOfLines.Add($"@Test");
                    listOfLines.Add($"public void validate_Page_{control.Name}_Number_Of_Columns() {{");
                    listOfLines.Add($"asserts.greaterThan({page.Name.CamelCase()}.{control.Name.CamelCase()}.getNumberOfColumns(), -1, \"Validating the {page.Name} {control.Name} number of columns...\");");
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
                                listOfLines.Add($"@Test");
                                listOfLines.Add($"public void validate_Page_{control.Name}_Cell_Text_{name}() {{");
                                listOfLines.Add($"asserts.isNotNull({page.Name.CamelCase()}.{control.Name.CamelCase()}.getCellText(1, \"{name}\"), \"Validating the {page.Name} {control.Name} table cell Text {name}...\");");
                                listOfLines.Add($"}}");
                            }
                        }
                    }
                    else
                    {
                        listOfLines.Add($"");
                        listOfLines.Add($"@Test");
                        listOfLines.Add($"public void validate_Page_{control.Name}_Cell_Text() {{");
                        listOfLines.Add($"asserts.isNotNull({page.Name.CamelCase()}.{control.Name.CamelCase()}.getCellText(1, 2), \"Validating the {page.Name} {control.Name} table cell Text...\");");
                        listOfLines.Add($"}}");
                    }
                }
            }

            return listOfLines;
        }
    }
}
