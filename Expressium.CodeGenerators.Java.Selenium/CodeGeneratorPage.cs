using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Expressium.CodeGenerators.Java.Selenium
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
                return Path.Combine(GetMainSourcePath(), CodeFolders.pages.ToString(), page.Name + ".java");
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
            listOfLines.AddRange(GenerateClass(page));
            listOfLines.Add($"{{");
            listOfLines.AddRange(GenerateMembers(page));
            listOfLines.AddRange(GenerateLocators(page));
            listOfLines.AddRange(GenerateContructor(page));
            listOfLines.AddRange(GenerateActionMethods(page));
            listOfLines.AddRange(GenerateFillFormMethod(page));
            listOfLines.AddRange(GenerateExtensionMethods(page));
            listOfLines.Add($"}}");

            return listOfLines;
        }

        internal List<string> GeneratePackage()
        {
            return new List<string>
            {
                $"package {GetPackage()}.{CodeFolders.pages};"
            };
        }

        internal List<string> GenerateImports(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add("import org.openqa.selenium.*;");
            listOfLines.Add($"import {GetPackage()}.{CodeFolders.controls}.*;");

            if (page.Model)
                listOfLines.Add($"import {GetPackage()}.{CodeFolders.models}.*;");

            listOfLines.Add("import org.slf4j.Logger;");

            return listOfLines;
        }

        internal List<string> GenerateClass(ObjectRepositoryPage page)
        {
            var basePage = "BasePage";
            if (!string.IsNullOrWhiteSpace(page.Base))
                basePage = page.Base;

            return new List<string>
            {
                $"public class {page.Name} extends {basePage}"
            };
        }

        internal List<string> GenerateMembers(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var member in page.Members)
                listOfLines.Add($"public {member.Page} {member.Name.CamelCase()};");

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                    listOfLines.Add($"public BaseTable {control.Name.CamelCase()};");
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

            return new List<string>
            {
                $"private Web{type} {control.Name.CamelCase()};"
            };
        }

        internal List<string> GenerateContructor(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public {page.Name}(Logger logger, WebDriver driver) {{");
            listOfLines.Add($"super(logger, driver);");

            foreach (var member in page.Members)
                listOfLines.Add($"this.{member.Name.CamelCase()} = new {member.Page}(logger, driver);");

            foreach (var control in page.Controls)
            {
                if (control.IsTable())
                    listOfLines.Add($"this.{control.Name.CamelCase()} = new BaseTable(logger, driver, By.{GetJavaByMethod(control.How)}(\"{control.Using}\"));");
            }

            foreach (var control in page.Controls)
            {
                if (!control.IsTable())
                {
                    var type = control.Type;
                    if (type == ControlTypes.Element.ToString())
                        type = "Control";
                    listOfLines.Add($"this.{control.Name.CamelCase()} = new Web{type}(driver, By.{GetJavaByMethod(control.How)}(\"{control.Using.EscapeDoubleQuotes()}\"));");
                }
            }

            if (page.Synchronizers.Count > 0 && (page.Members.Count > 0 || page.Controls.Count > 0))
                listOfLines.Add("");

            foreach (var synchronizer in page.Synchronizers)
            {
                if (synchronizer.How == SynchronizerTypes.WaitForPageElementIsVisible.ToString() ||
                    synchronizer.How == SynchronizerTypes.WaitForPageElementIsEnabled.ToString())
                {
                    listOfLines.Add($"{synchronizer.How.CamelCase()}({synchronizer.Using});");
                }
                else
                {
                    listOfLines.Add($"{synchronizer.How.CamelCase()}(\"{synchronizer.Using}\");");
                }
            }

            listOfLines.Add($"}}");
            listOfLines.Add($"");

            return listOfLines;
        }

        internal string GetJavaByMethod(string how)
        {
            switch (how)
            {
                case "Id": return "id";
                case "Name": return "name";
                case "XPath": return "xpath";
                case "CssSelector": return "cssSelector";
                case "ClassName": return "className";
                case "TagName": return "tagName";
                case "LinkText": return "linkText";
                case "PartialLinkText": return "partialLinkText";
                default: return how.ToLower();
            }
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
                listOfLines.Add($"public void fillForm({page.Name}Model model) {{");

                foreach (var control in page.Controls)
                {
                    if (control.IsFillFormControl())
                    {
                        if (control.IsCheckBox() || control.IsRadioButton())
                            listOfLines.Add($"set{control.Name}(model.is{control.Name}());");
                        else
                            listOfLines.Add($"set{control.Name}(model.get{control.Name}());");
                    }
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

        internal List<string> GenerateActionMethod(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            if (control.IsTextBox())
                listOfLines.AddRange(GenerateActionMethodTextBox(control));
            else if (control.IsComboBox() || control.IsListBox())
                listOfLines.AddRange(GenerateActionMethodComboBox(control));
            else if (control.IsCheckBox())
                listOfLines.AddRange(GenerateActionMethodCheckBox(control));
            else if (control.IsRadioButton())
                listOfLines.AddRange(GenerateActionMethodRadioBox(control));
            else if (control.IsLink() || control.IsButton())
                listOfLines.AddRange(GenerateActionMethodButton(control));
            else if (control.IsText())
                listOfLines.AddRange(GenerateActionMethodText(control));

            return listOfLines;
        }

        internal List<string> GenerateActionMethodTextBox(ObjectRepositoryControl control)
        {
            return new List<string>
            {
                $"public void set{control.Name}(String value) {{",
                $"logger.info(\"set{control.Name}(\" + value + \")\");",
                $"{control.Name.CamelCase()}.setText(value);",
                $"}}",
                "",
                $"public String get{control.Name}() {{",
                $"logger.info(\"get{control.Name}()\");",
                $"return {control.Name.CamelCase()}.getText();",
                $"}}",
                ""
            };
        }

        internal List<string> GenerateActionMethodComboBox(ObjectRepositoryControl control)
        {
            return new List<string>
            {
                $"public void set{control.Name}(String value) {{",
                $"logger.info(\"set{control.Name}(\" + value + \")\");",
                $"{control.Name.CamelCase()}.setText(value);",
                $"}}",
                "",
                $"public String get{control.Name}() {{",
                $"logger.info(\"get{control.Name}()\");",
                $"return {control.Name.CamelCase()}.getSelected();",
                $"}}",
                ""
            };
        }

        internal List<string> GenerateActionMethodCheckBox(ObjectRepositoryControl control)
        {
            return new List<string>
            {
                $"public void set{control.Name}(boolean value) {{",
                $"logger.info(\"set{control.Name}(\" + value + \")\");",
                $"{control.Name.CamelCase()}.setChecked(value);",
                $"}}",
                "",
                $"public boolean is{control.Name}() {{",
                $"logger.info(\"is{control.Name}()\");",
                $"return {control.Name.CamelCase()}.getChecked();",
                $"}}",
                ""
            };
        }

        internal List<string> GenerateActionMethodRadioBox(ObjectRepositoryControl control)
        {
            return new List<string>
            {
                $"public void set{control.Name}(boolean value) {{",
                $"logger.info(\"set{control.Name}(\" + value + \")\");",
                $"{control.Name.CamelCase()}.setSelected(value);",
                $"}}",
                "",
                $"public boolean is{control.Name}() {{",
                $"logger.info(\"is{control.Name}()\");",
                $"return {control.Name.CamelCase()}.getSelected();",
                $"}}",
                ""
            };
        }

        internal List<string> GenerateActionMethodButton(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public void click{control.Name}() {{");

            if (!string.IsNullOrEmpty(control.Source))
            {
                listOfLines.Add($"click{control.Source}();");
                listOfLines.Add("");
            }

            listOfLines.Add($"logger.info(\"click{control.Name}()\");");
            listOfLines.Add($"{control.Name.CamelCase()}.click();");
            listOfLines.Add($"}}");
            listOfLines.Add("");

            return listOfLines;
        }

        internal List<string> GenerateActionMethodText(ObjectRepositoryControl control)
        {
            return new List<string>
            {
                $"public String get{control.Name}() {{",
                $"logger.info(\"get{control.Name}()\");",
                $"return {control.Name.CamelCase()}.getText();",
                $"}}",
                ""
            };
        }
    }
}
