using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorPageCSharp : CodeGeneratorPage
    {
        internal CodeGeneratorPageCSharp(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetFilePath(ObjectRepositoryPage page)
        {
            try
            {
                return Path.Combine(configuration.SolutionPath, configuration.GetNameSpace(), CodeFolders.Pages.ToString(), page.Name + ".cs");
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
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateMembers(page));
            listOfLines.AddRange(GenerateLocators(page));
            listOfLines.AddRange(GenerateContructor(page));
            listOfLines.AddRange(GenerateControlMethods(page));
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

            if (!configuration.IsCodingStyleByLocators())
                listOfLines.Add("using SeleniumExtras.PageObjects;");

            if (page.Model)
                listOfLines.Add($"using {configuration.GetNameSpace()}.Models;");

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
                $"namespace {configuration.GetNameSpace()}.{CodeFolders.Pages}"
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

            if (configuration.IsCodingStyleByLocators())
            {
                foreach (var control in page.Controls)
                {
                    if (!control.IsTable())
                        listOfLines.AddRange(GenerateByLocator(control));
                }

                if (listOfLines.Count > 0)
                    listOfLines.Add("");
            }
            else
            {
                foreach (var control in page.Controls)
                {
                    if (!control.IsTable())
                        listOfLines.AddRange(GenerateFindsByLocator(control));
                }
            }

            return listOfLines;
        }

        internal List<string> GenerateFindsByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"[FindsBy(How = How.{control.How}, Using = \"{control.Using}\")]",
                $"private IWebElement {control.Name} {{ get; set; }}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private readonly By {control.Name} = By.{control.How}(\"{control.Using}\");"
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

        internal List<string> GenerateCheckBoxMethods(ObjectRepositoryControl control)
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

        internal List<string> GenerateButtonMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public void Click{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"Click{control.Name}()\");",
                $"{control.Name}.Click{control.Type}(driver);",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateTextMethods(ObjectRepositoryControl control)
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
    }
}
