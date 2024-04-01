using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;

namespace Expressium.CodeGenerators.Java
{
    internal class CodeGeneratorControlJava
    {
        internal static string GetFindbysHowMapping(string how)
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

        internal static List<string> GenerateFindsByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"@FindBy(how = How.{GetFindbysHowMapping(control.How)}, using = \"{control.Using}\")",
                $"private WebElement {control.Name.CamelCase()};",
                ""
            };

            return listOfLines;
        }

        internal static List<string> GenerateByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private By {control.Name.CamelCase()} = By.{control.How.ToLower()}(\"{control.Using}\");"
            };

            return listOfLines;
        }

        internal static List<string> GenerateMemberMethod(ObjectRepositoryMember member)
        {
            var listOfLines = new List<string>
            {
                $"public {member.Page} get{member.Page}()",
                $"{{",
                $"return {member.Name};",
                $"}}",
                ""
            };

            return listOfLines;
        }

        internal static List<string> GenerateMethod(ObjectRepositoryControl control)
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

        internal static List<string> GenerateTextBoxMethods(ObjectRepositoryControl control)
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

        internal static List<string> GenerateCheckBoxMethods(ObjectRepositoryControl control)
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

        internal static List<string> GenerateButtonMethods(ObjectRepositoryControl control)
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

        internal static List<string> GenerateTextMethods(ObjectRepositoryControl control)
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
    }
}
