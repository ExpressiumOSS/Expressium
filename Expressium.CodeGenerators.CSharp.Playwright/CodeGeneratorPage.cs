using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators.CSharp.Playwright
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

            listOfLines.Add("using Microsoft.Playwright;");
            listOfLines.Add($"using {GetNameSpace()}.Controls;");

            if (page.Model)
                listOfLines.Add($"using {GetNameSpace()}.Models;");

            listOfLines.Add("using System;");
            listOfLines.Add("using System.Collections.Generic;");
            listOfLines.Add("using log4net;");
            listOfLines.Add("using System.Threading.Tasks;");
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

            foreach (var control in page.Controls)
            {
                if (!control.IsTable())
                    listOfLines.AddRange(GenerateLocator(control));
            }

            if (listOfLines.Count > 0)
                listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateLocator(ObjectRepositoryControl control)
        {
            var type = control.Type;
            if (type == ControlTypes.Element.ToString())
                type = "Control";

            var listOfLines = new List<string>();

            if (control.How == ControlHows.Label.ToString())
            {
                var ariaRoleType = GetAriaRoleType(type);
            
                listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.GetByRole(AriaRole.{ariaRoleType}, new() {{ Name = \"{control.Using.EscapeDoubleQuotes()}\", Exact = true }}));");
            }
            else if (control.How == ControlHows.Id.ToString())
            {
                //listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.Locator(\"#{control.Using.EscapeDoubleQuotes()}\"));");
                listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.Locator(\"[{control.How.ToLower()}='{control.Using.EscapeDoubleQuotes()}']\"));");
            }
            else if (control.How == ControlHows.Name.ToString())
            {
                listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.Locator(\"[{control.How.ToLower()}='{control.Using.EscapeDoubleQuotes()}']\"));");
            }
            else if (control.How == ControlHows.ClassName.ToString())
            {
                listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.Locator(\".{control.Using.EscapeDoubleQuotes()}\"));");
            }
            else if (control.How == ControlHows.CssSelector.ToString())
            {
                listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.Locator(\"{control.Using.EscapeDoubleQuotes()}\"));");
            }
            else if (control.How == ControlHows.XPath.ToString())
            {
                listOfLines.Add($"private Web{type} {control.Name} => new Web{type}(page, page.Locator(\"{control.Using.EscapeDoubleQuotes()}\"));");
            }

            return listOfLines;
        }

        internal List<string> GenerateContructor(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public {page.Name}(ILog logger, IPage page) : base(logger, page)");
            listOfLines.Add($"{{");

            foreach (var member in page.Members)
                listOfLines.Add($"{member.Name} = new {member.Page}(logger, page);");

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                {
                    if (control.How == ControlHows.Id.ToString())
                    {
                        listOfLines.Add($"{control.Name} = new BaseTable(logger, page, page.Locator(\"#{control.Using.EscapeDoubleQuotes()}\"));");
                    }
                    else if (control.How == ControlHows.Name.ToString())
                    {
                        listOfLines.Add($"{control.Name} = new BaseTable(logger, page, page.Locator(\"[{control.How.ToLower()}='{control.Using.EscapeDoubleQuotes()}']\"));");
                    }
                    else if (control.How == ControlHows.ClassName.ToString())
                    {
                        listOfLines.Add($"{control.Name} = new BaseTable(logger, page, page.Locator(\".{control.Using.EscapeDoubleQuotes()}\"));");
                    }
                    else if (control.How == ControlHows.CssSelector.ToString())
                    {
                        listOfLines.Add($"{control.Name} = new BaseTable(logger, page, page.Locator(\"{control.Using.EscapeDoubleQuotes()}\"));");
                    }
                    else if (control.How == ControlHows.XPath.ToString())
                    {
                        listOfLines.Add($"{control.Name} = new BaseTable(logger, page, page.Locator(\"{control.Using.EscapeDoubleQuotes()}\"));");
                    }
                }
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

            foreach (var control in page.Controls)
                listOfLines.AddRange(GenerateActionMethod(control));

            return listOfLines;
        }

        internal List<string> GenerateFillFormMethod(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            if (page.Model)
            {
                listOfLines.Add($"public async Task FillForm({page.Name}Model model)");
                listOfLines.Add($"{{");

                foreach (var control in page.Controls)
                {
                    if (control.IsFillFormControl())
                        listOfLines.Add($"await Set{control.Name}({"model." + control.Name});");
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

        internal List<string> GenerateActionMethod(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            if (control.IsTextBox())
            {
                listOfLines.AddRange(GenerateActionMethodTextBox(control));
            }
            else if (control.IsComboBox() || control.IsListBox())
            {
                listOfLines.AddRange(GenerateActionMethodComboBox(control));
            }
            else if (control.IsCheckBox())
            {
                listOfLines.AddRange(GenerateActionMethodCheckBox(control));
            }
            else if (control.IsRadioButton())
            {
                listOfLines.AddRange(GenerateActionMethodRadioBox(control));
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
                $"public async Task Set{control.Name}(string value)",
                $"{{",
                $"logger.Info($\"Set{control.Name}({{value}})\");",
                $"await {control.Name}.SetText(value);",
                $"}}",
                "",
                $"public async Task<string> Get{control.Name}()",
                $"{{",
                $"logger.Info(\"Get{control.Name}()\");",
                $"return await {control.Name}.GetText();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodComboBox(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public async Task Set{control.Name}(string value)",
                $"{{",
                $"logger.Info($\"Set{control.Name}({{value}})\");",
                $"await {control.Name}.SetText(value);",
                $"}}",
                "",
                $"public async Task<string> Get{control.Name}()",
                $"{{",
                $"logger.Info(\"Get{control.Name}()\");",
                $"return await {control.Name}.GetSelected();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodCheckBox(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public async Task Set{control.Name}(bool value)",
                $"{{",
                $"logger.Info($\"Set{control.Name}({{value}})\");",
                $"await {control.Name}.SetChecked(value);",
                $"}}",
                "",
                $"public async Task<bool> Get{control.Name}()",
                $"{{",
                $"logger.Info(\"Get{control.Name}()\");",
                $"return await {control.Name}.GetChecked();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodRadioBox(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public async Task Set{control.Name}(bool value)",
                $"{{",
                $"logger.Info($\"Set{control.Name}({{value}})\");",
                $"await {control.Name}.SetSelected(value);",
                $"}}",
                "",
                $"public async Task<bool> Get{control.Name}()",
                $"{{",
                $"logger.Info(\"Get{control.Name}()\");",
                $"return await {control.Name}.GetSelected();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal List<string> GenerateActionMethodButton(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public async Task Click{control.Name}()");
            listOfLines.Add($"{{");

            if (!string.IsNullOrEmpty(control.Source))
            {
                listOfLines.Add($"Click{control.Source}();");
                listOfLines.Add("");
            }

            listOfLines.Add($"logger.Info(\"Click{control.Name}()\");");
            listOfLines.Add($"await {control.Name}.Click();");
            listOfLines.Add($"}}");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateActionMethodText(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public async Task<string> Get{control.Name}()",
                $"{{",
                $"logger.Info(\"Get{control.Name}()\");",
                $"return await {control.Name}.GetText();",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal string GetAriaRoleType(string type)
        {
            string role = null;

            if (type.StartsWith(ControlTypes.Button.ToString()))
                role = "Button";
            else if (type.StartsWith(ControlTypes.Link.ToString()))
                role = "Link";
            else if (type.StartsWith(ControlTypes.RadioButton.ToString()))
                role = "Radio";
            else if (type.StartsWith(ControlTypes.CheckBox.ToString()))
                role = "Checkbox";
            else if (type.StartsWith(ControlTypes.ComboBox.ToString()))
                role = "Combobox";
            else if (type.StartsWith(ControlTypes.ListBox.ToString()))
                role = "Listbox";
            else if (type.StartsWith(ControlTypes.TextBox.ToString()))
                role = "Textbox";
            else if (type.StartsWith(ControlTypes.Text.ToString()))
                role = "Text";
            else if (type.StartsWith(ControlTypes.Table.ToString()))
                role = "Table";
            else
            {
            }

            return role;
        }
    }
}