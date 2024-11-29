using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorPage : CodeGeneratorObject
    {
        internal CodeGeneratorPage(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }
        public void Generate(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);
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
                return Path.Combine(configuration.SolutionPath, @"src\main\java", CodeFolders.Pages.ToString(), page.Name + ".java");
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
            listOfLines.AddRange(GenerateMembers(page));
            listOfLines.AddRange(GenerateLocators(page));
            listOfLines.AddRange(GenerateContructor(page));
            listOfLines.AddRange(GenerateMemberMethods(page));
            listOfLines.AddRange(GenerateActionMethods(page));
            listOfLines.AddRange(GenerateFillFormMethod(page));
            listOfLines.AddRange(GenerateExtensionMethods(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateImports(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("package Pages;");
            listOfLines.Add("");
            listOfLines.Add("import Bases.*;");

            if (IsCodingStyleByControls())
                listOfLines.Add("import Controls.*;");

            listOfLines.Add("import Models.*;");
            listOfLines.Add("import org.openqa.selenium.By;");
            listOfLines.Add("import org.openqa.selenium.Keys;");
            listOfLines.Add("import org.openqa.selenium.WebDriver;");
            listOfLines.Add("import org.openqa.selenium.WebElement;");
            listOfLines.Add("import org.openqa.selenium.support.FindBy;");
            listOfLines.Add("import org.openqa.selenium.support.How;");
            listOfLines.Add("");

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

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                    listOfLines.Add($"private BaseTable {control.Name} = new BaseTable(driver, By.{control.How.ToLower()}(\"{control.Using}\"));");
            }

            if (page.Members.Count > 0 || page.Controls.Any(c => c.IsTable()))
                listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateLocators(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            if (IsCodingStylePageFactory())
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(GeneratePageFactoryLocator(control));
            }
            else if (IsCodingStyleByLocators())
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(GenerateByLocatorsLocator(control));
            }
            else if (IsCodingStyleByControls())
            {
                foreach (var control in page.Controls)
                {
                    if (!control.IsTable())
                        listOfLines.AddRange(GenerateByControlsLocator(control));
                }
            }

            if (page.Controls.Count > 0)
                listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GeneratePageFactoryLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"@FindBy(how = How.{GetFindbysHowMapping(control.How)}, using = \"{control.Using}\")",
                $"private WebElement {control.Name.CamelCase()};"
            };

            return listOfLines;
        }

        internal List<string> GenerateByLocatorsLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private By {control.Name.CamelCase()} = By.{control.How.ToLower()}(\"{control.Using}\");"
            };

            return listOfLines;
        }

        internal List<string> GenerateByControlsLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private Web{control.Type} {control.Name.CamelCase()} = new Web{control.Type}(driver, By.{control.How.ToLower()}(\"{control.Using}\"));"
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

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                {
                    listOfLines.Add($"public BaseTable get{control.Name}()");
                    listOfLines.Add($"{{");
                    listOfLines.Add($"return {control.Name};");
                    listOfLines.Add($"}}");
                    listOfLines.Add("");
                }
                    
            }

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

        internal List<string> GenerateActionMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            if (IsCodingStyleByControls())
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(GenerateActionMethodByControls(control));
            }
            else
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(GenerateActionMethod(control));
            }

            return listOfLines;
        }

        internal List<string> GenerateActionMethod(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            if (control.IsTextBox() || control.IsComboBox() || control.IsListBox())
            {
                listOfLines.AddRange(GenerateActionMethodTextBox(control));
            }
            else if (control.IsRadioButton() || control.IsCheckBox())
            {
                listOfLines.AddRange(GenerateActionMethodCheckBox(control));
            }
            else if (control.IsLink() || control.IsButton())
            {
                listOfLines.AddRange(GenerateActionMethodButton(control));
            }
            else if (control.IsText())
            {
                listOfLines.AddRange(GenerateActionMethodText(control));
            }
            else
            {
            }

            return listOfLines;
        }

        internal List<string> GenerateActionMethodTextBox(ObjectRepositoryControl control)
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

        internal List<string> GenerateActionMethodCheckBox(ObjectRepositoryControl control)
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

        internal List<string> GenerateActionMethodButton(ObjectRepositoryControl control)
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

        internal List<string> GenerateActionMethodText(ObjectRepositoryControl control)
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

        internal List<string> GenerateActionMethodByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            if (control.IsTextBox())
            {
                listOfLines.AddRange(GenerateActionMethodTextBoxByControls(control));
            }
            else if (control.IsComboBox() || control.IsListBox())
            {
                listOfLines.AddRange(GenerateActionMethodComboBoxByControls(control));
            }
            else if (control.IsCheckBox())
            {
                listOfLines.AddRange(GenerateActionMethodCheckBoxByControls(control));
            }
            else if (control.IsRadioButton())
            {
                listOfLines.AddRange(GenerateActionMethodRadioBoxByControls(control));
            }
            else if (control.IsLink() || control.IsButton())
            {
                listOfLines.AddRange(GenerateActionMethodButtonByControls(control));
            }
            else if (control.IsText())
            {
                listOfLines.AddRange(GenerateActionMethodTextByControls(control));
            }
            else
            {
            }

            return listOfLines;
        }

        internal List<string> GenerateActionMethodTextBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void set{control.Name}(String value) throws Exception",
                $"{{",
                $"logger.info(String.format(\"set{control.Name}(%s)\", value));",
                $"{control.Name.CamelCase()}.setText(value);",
                $"}}",
                "",
                $"public String get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return {control.Name.CamelCase()}.getText();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodComboBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void set{control.Name}(String value) throws Exception",
                $"{{",
                $"logger.info(String.format(\"set{control.Name}(%s)\", value));",
                $"{control.Name.CamelCase()}.selectByText(value);",
                $"}}",
                "",
                $"public String get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return {control.Name.CamelCase()}.getSelectedText();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodCheckBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void set{control.Name}(boolean value) throws Exception",
                $"{{",
                $"logger.info(String.format(\"set{control.Name}(%s)\", value));",
                $"{control.Name.CamelCase()}.setChecked(value);",
                $"}}",
                "",
                $"public boolean get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return {control.Name.CamelCase()}.getChecked();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodRadioBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void set{control.Name}(boolean value) throws Exception",
                $"{{",
                $"logger.info(String.format(\"set{control.Name}(%s)\", value));",
                $"{control.Name.CamelCase()}.setSelected(value);",
                $"}}",
                "",
                $"public boolean get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return {control.Name.CamelCase()}.getSelected();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodButtonByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void click{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"click{control.Name}()\"));",
                $"{control.Name.CamelCase()}.click();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodTextByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public String get{control.Name}() throws Exception",
                $"{{",
                $"logger.info(String.format(\"get{control.Name}()\"));",
                $"return {control.Name.CamelCase()}.getText();",
                $"}}",
                ""
            };

            return listOfLines;
        }
    }
}
