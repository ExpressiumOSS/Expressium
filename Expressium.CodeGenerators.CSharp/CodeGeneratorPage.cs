using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorPage : CodeGeneratorObject
    {
        internal CodeGeneratorPage(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
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
                return Path.Combine(configuration.SolutionPath, GetNameSpace(), CodeFolders.Pages.ToString(), page.Name + ".cs");
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
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateMembers(page));
            listOfLines.AddRange(GenerateLocators(page));
            listOfLines.AddRange(GenerateContructor(page));
            listOfLines.AddRange(GenerateActionMethods(page));
            listOfLines.AddRange(GenerateFillFormMethod(page));
            listOfLines.AddRange(GenerateExtensionMethods(page));
            listOfLines.Add($"}}");
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GenerateUsings(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("using OpenQA.Selenium;");

            if (IsCodingStylePageFactory())
                listOfLines.Add("using SeleniumExtras.PageObjects;");

            if (IsCodingStyleByControls())
                listOfLines.Add($"using {GetNameSpace()}.Controls;");

            if (page.Model)
                listOfLines.Add($"using {GetNameSpace()}.Models;");

            listOfLines.Add("using System;");
            listOfLines.Add("using System.Collections.Generic;");
            listOfLines.Add("using log4net;");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateNameSpace()
        {
            var listOfLines = new List<string>
            {
                $"namespace {GetNameSpace()}.{CodeFolders.Pages}"
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
                $"public partial class {page.Name} : {basePage}"
            };

            return listOfLines;
        }

        internal List<string> GenerateMembers(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var member in page.Members)
                listOfLines.Add($"public {member.Page} {member.Name} {{ get; private set; }}");

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                    listOfLines.Add($"public BaseTable {control.Name} {{ get; private set; }}");
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
                {
                    if (!control.IsTable())
                        listOfLines.AddRange(GeneratePageFactoryLocator(control));
                }
            }
            else if (IsCodingStyleByLocators())
            {
                foreach (var control in page.Controls)
                {
                    if (!control.IsTable())
                        listOfLines.AddRange(GenerateByLocatorsLocator(control));
                }
            }
            else if (IsCodingStyleByControls())
            {
                foreach (var control in page.Controls)
                {
                    if (!control.IsTable())
                        listOfLines.AddRange(GenerateByControlsLocator(control));
                }
            }
            else
            {
            }

            if (listOfLines.Count > 0)
                listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GeneratePageFactoryLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"[FindsBy(How = How.{control.How}, Using = \"{control.Using.EscapeDoubleQuotes()}\")]",
                $"private IWebElement {control.Name} {{ get; set; }}"
            };

            return listOfLines;
        }

        internal List<string> GenerateByLocatorsLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private readonly By {control.Name} = By.{control.How}(\"{control.Using.EscapeDoubleQuotes()}\");"
            };

            return listOfLines;
        }

        internal List<string> GenerateByControlsLocator(ObjectRepositoryControl control)
        {
            var type = control.Type;
            if (type == ControlTypes.Element.ToString())
                type = "Control";

            var listOfLines = new List<string>
            {
                $"private Web{type} {control.Name} => new Web{type}(driver, By.{control.How}(\"{control.Using.EscapeDoubleQuotes()}\"));"
            };

            return listOfLines;
        }

        internal List<string> GenerateContructor(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public {page.Name}(ILog logger, IWebDriver driver) : base(logger, driver)");
            listOfLines.Add($"{{");

            foreach (var member in page.Members)
                listOfLines.Add($"{member.Name} = new {member.Page}(logger, driver);");

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                    listOfLines.Add($"{control.Name} = new BaseTable(logger, driver, By.{control.How}(\"{control.Using}\"));");
            }

            if (page.Synchronizers.Count > 0 && (page.Members.Count > 0 || page.Controls.Any(c => c.IsTable())))
                listOfLines.Add("");

            foreach (var synchronizer in page.Synchronizers)
            {
                if (synchronizer.How == SynchronizerTypes.WaitForPageElementIsVisible.ToString() ||
                    synchronizer.How == SynchronizerTypes.WaitForPageElementIsEnabled.ToString())
                {
                    listOfLines.Add($"{synchronizer.How}({synchronizer.Using});");
                }
                else
                {
                    listOfLines.Add($"{synchronizer.How}(\"{synchronizer.Using}\");");
                }
            }

            listOfLines.Add($"}}");
            listOfLines.Add($"");

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
                $"public void Set{control.Name}(string value)",
                $"{{",
                $"logger.InfoFormat(\"Set{control.Name}({{0}})\", value);",
                $"{control.Name}.Set{control.Type}(driver, value);",
                $"}}",
                "",
                $"public string Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.Get{control.Type}(driver);",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodCheckBox(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void Set{control.Name}(bool value)",
                $"{{",
                $"logger.InfoFormat(\"Set{control.Name}({{0}})\", value);",
                $"{control.Name}.Set{control.Type}(driver, value);",
                $"}}",
                "",
                $"public bool Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.Get{control.Type}(driver);",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodButton(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public void Click{control.Name}()");
            listOfLines.Add($"{{");

            if (!string.IsNullOrEmpty(control.Source))
            {
                listOfLines.Add($"Click{control.Source}();");
                listOfLines.Add("");
            }

            listOfLines.Add($"logger.InfoFormat(\"Click{control.Name}()\");");
            listOfLines.Add($"{control.Name}.Click{control.Type}(driver);");
            listOfLines.Add($"}}");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateActionMethodText(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public string Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.GetText(driver);",
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
                listOfLines.Add($"public void FillForm({page.Name}Model model)");
                listOfLines.Add($"{{");

                foreach (var control in page.Controls)
                {
                    if (control.IsFillFormControl())
                        listOfLines.Add($"Set{control.Name}({"model." + control.Name});");
                }

                listOfLines.Add($"}}");
                listOfLines.Add("");
            }

            return listOfLines;
        }

        internal List<string> GenerateExtensionMethods(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);
            return GenerateSourceCodeExtensionMethods(filePath, "#region Extensions", "#endregion");
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
                $"public void Set{control.Name}(string value)",
                $"{{",
                $"logger.InfoFormat(\"Set{control.Name}({{0}})\", value);",
                $"{control.Name}.SetText(value);",
                $"}}",
                "",
                $"public string Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.GetText();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodComboBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void Set{control.Name}(string value)",
                $"{{",
                $"logger.InfoFormat(\"Set{control.Name}({{0}})\", value);",
                $"{control.Name}.SelectByText(value);",
                $"}}",
                "",
                $"public string Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.GetSelectedText();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodCheckBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void Set{control.Name}(bool value)",
                $"{{",
                $"logger.InfoFormat(\"Set{control.Name}({{0}})\", value);",
                $"{control.Name}.SetChecked(value);",
                $"}}",
                "",
                $"public bool Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.GetChecked();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodRadioBoxByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void Set{control.Name}(bool value)",
                $"{{",
                $"logger.InfoFormat(\"Set{control.Name}({{0}})\", value);",
                $"{control.Name}.SetSelected(value);",
                $"}}",
                "",
                $"public bool Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.GetSelected();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodButtonByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public void Click{control.Name}()");
            listOfLines.Add($"{{");

            if (!string.IsNullOrEmpty(control.Source))
            {
                listOfLines.Add($"Click{control.Source}();");
                listOfLines.Add("");
            }

            listOfLines.Add($"logger.InfoFormat(\"Click{control.Name}()\");");
            listOfLines.Add($"{control.Name}.Click();");
            listOfLines.Add($"}}");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateActionMethodTextByControls(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public string Get{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}()\");",
                $"return {control.Name}.GetText();",
                $"}}",
                ""
            };

            return listOfLines;
        }
    }
}