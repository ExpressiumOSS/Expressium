using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorPageJava : CodeGeneratorPage
    {
        internal CodeGeneratorPageJava(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, @"src\main\java", CodeFolders.Pages.ToString(), page.Name + ".java");
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
            listOfLines.AddRange(GenerateMembers(page));
            listOfLines.AddRange(GenerateLocators(page));
            listOfLines.AddRange(GenerateContructor(page));
            listOfLines.AddRange(GenerateMemberMethods(page));
            listOfLines.AddRange(GenerateControlMethods(page));
            listOfLines.AddRange(GenerateFillFormMethod(page));
            listOfLines.AddRange(GenerateExtensionMethods(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateImports(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                "package Pages;",
                "",
                "import Bases.BasePage;",
                "import Bases.WebElements;",
                "import Models.*;",
                "import org.openqa.selenium.By;",
                "import org.openqa.selenium.Keys;",
                "import org.openqa.selenium.WebDriver;",
                "import org.openqa.selenium.WebElement;",
                "import org.openqa.selenium.support.FindBy;",
                "import org.openqa.selenium.support.How;",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            var basePage = "BasePage";
            if (!string.IsNullOrWhiteSpace(page.Base))
                basePage = page.Base;

            var listOfLines = new List<string>
            {
                $"public class {page.Name} extends {basePage}"
            };

            return listOfLines;
        }

        internal List<string> GenerateMembers(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var member in page.Members)
                listOfLines.Add($"private {member.Page} {member.Name};");

            if (page.Members.Count > 0)
                listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateLocators(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            if (configuration.IsCodingStyleByLocators())
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(GenerateByLocator(control));

                if (page.Controls.Count > 0)
                    listOfLines.Add("");
            }
            else
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(GenerateFindsByLocator(control));
            }

            return listOfLines;
        }

        internal List<string> GenerateFindsByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"@FindBy(how = How.{GetFindbysHowMapping(control.How)}, using = \"{control.Using}\")",
                $"private WebElement {control.Name.CamelCase()};",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private By {control.Name.CamelCase()} = By.{control.How.ToLower()}(\"{control.Using}\");"
            };

            return listOfLines;
        }


        internal string GetFindbysHowMapping(string how)
        {
            if (how == ControlHows.ClassName.ToString())
                return "CLASS_NAME";
            else if (how == ControlHows.CssSelector.ToString())
                return "CSS";
            else if (how == ControlHows.LinkText.ToString())
                return "LINK_TEXT";
            else if (how == ControlHows.PartialLinkText.ToString())
                return "PARTIAL_LINK_TEXT";
            else if (how == ControlHows.TagName.ToString())
                return "TAG_NAME";
            else
            {
            }

            return how.ToUpper();
        }

        internal List<string> GenerateContructor(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>
            {
                $"public {page.Name}(WebDriver driver) throws Exception",
                $"{{",
                $"super(driver);"
            };

            if (page.Members.Count > 0)
            {
                listOfLines.Add("");
                foreach (var member in page.Members)
                    listOfLines.Add($"{member.Name} = new {member.Page}(driver);");
            }

            if (page.Synchronizers.Count > 0)
            {
                listOfLines.Add("");
                foreach (var synchronizer in page.Synchronizers)
                {
                    if (synchronizer.How == SynchronizerTypes.WaitForPageElementIsVisible.ToString() ||
                        synchronizer.How == SynchronizerTypes.WaitForPageElementIsEnabled.ToString())
                    {
                        listOfLines.Add($"{synchronizer.How.CamelCase()}({synchronizer.Using.CamelCase()});");
                    }
                    else
                    {
                        listOfLines.Add($"{synchronizer.How.CamelCase()}(\"{synchronizer.Using}\");");
                    }
                }
            }

            listOfLines.Add($"}}");
            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GenerateMemberMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var member in page.Members)
            {
                listOfLines.Add($"public {member.Page} get{member.Page}()");
                listOfLines.Add($"{{");
                listOfLines.Add($"return {member.Name};");
                listOfLines.Add($"}}");
                listOfLines.Add("");
            }

            return listOfLines;
        }

        internal List<string> GenerateControlMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
                listOfLines.AddRange(GenerateMethod(control));

            return listOfLines;
        }

        internal List<string> GenerateMethod(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
            {
                listOfLines.AddRange(GenerateTextBoxMethods(control));
            }
            else if (control.IsRadioButton() || control.IsCheckBox())
            {
                listOfLines.AddRange(GenerateCheckBoxMethods(control));
            }
            else if (control.IsLink() || control.IsButton())
            {
                listOfLines.AddRange(GenerateButtonMethods(control));
            }
            else if (control.IsText())
            {
                listOfLines.AddRange(GenerateTextMethods(control));
            }
            else
            {
            }

            return listOfLines;
        }

        internal List<string> GenerateTextBoxMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void set{control.Name}(String value) throws Exception",
                $"{{",
                $"logger.info(String.format(\"set{control.Name}(%s)\", value));",
                $"WebElements.set{control.Type}(driver, {control.Name.CamelCase()}, value);",
                $"}}",
                "",
                $"public String get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return WebElements.get{control.Type}(driver, {control.Name.CamelCase()});",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateCheckBoxMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void set{control.Name}(boolean value) throws Exception",
                $"{{",
                $"logger.info(String.format(\"set{control.Name}(%s)\", value));",
                $"WebElements.set{control.Type}(driver, {control.Name.CamelCase()}, value);",
                $"}}",
                "",
                $"public boolean get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return WebElements.get{control.Type}(driver, {control.Name.CamelCase()});",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateButtonMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void click{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"click{control.Name}()\"));",
                $"WebElements.click{control.Type}(driver, {control.Name.CamelCase()});",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateTextMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public String get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return WebElements.getText(driver, {control.Name.CamelCase()});",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateFillFormMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            if (page.Model)
            {
                listOfLines.Add($"public void fillForm({page.Name}Model model) throws Exception");
                listOfLines.Add($"{{");

                foreach (var control in page.Controls)
                {
                    if (control.IsFillFormControl())
                        listOfLines.Add($"set{control.Name}(model.get{control.Name}());");
                }

                listOfLines.Add($"}}");
                listOfLines.Add("");
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
