using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.Linq;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorControlCSharp
    {
        internal static List<string> GenerateFindsByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"[FindsBy(How = How.{control.How}, Using = \"{control.Using}\")]",
                $"private IWebElement {control.Name} {{ get; set; }}",
                ""
            };

            return listOfLines;
        }

        internal static List<string> GenerateByLocator(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"private readonly By {control.Name} = By.{control.How}(\"{control.Using}\");"
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
            else if (control.IsTable())
            {
                listOfLines.AddRange(GenerateTableMethods(control));
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

        internal static List<string> GenerateCheckBoxMethods(ObjectRepositoryControl control)
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

        internal static List<string> GenerateButtonMethods(ObjectRepositoryControl control)
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

        internal static List<string> GenerateTextMethods(ObjectRepositoryControl control)
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

        internal static List<string> GenerateTableMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public int Get{control.Name}NumberOfRows()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}NumberOfRows()\");",
                $"return {control.Name}.GetSubElements(driver, By.XPath(\"./tbody/tr\")).Count;",
                $"}}",
                "",
                $"public int Get{control.Name}NumberOfColumns()",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}NumberOfColumns()\");",
                $"return {control.Name}.GetSubElements(driver, By.XPath(\"./thead/tr/th\")).Count;",
                $"}}",
                "",
                $"public void Click{control.Name}Cell(int rowIndex, int columnIndex)",
                $"{{",
                $"logger.InfoFormat(\"Click{control.Name}Cell({{0}}, {{1}})\", rowIndex, columnIndex);",
                $"var element = {control.Name}.GetSubElement(driver, By.XPath($\"./tbody/tr[{{rowIndex}}]/td[{{columnIndex}}]\"));",
                $"element.Click(driver);",
                $"}}",
                "",
                $"public string Get{control.Name}CellText(int rowIndex, int columnIndex)",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}CellText({{0}}, {{1}})\", rowIndex, columnIndex);",
                $"var element = {control.Name}.GetSubElement(driver, By.XPath($\"./tbody/tr[{{rowIndex}}]/td[{{columnIndex}}]\"));",
                $"return element.GetText(driver);",
                $"}}",
                "",
                $"public string Get{control.Name}CellText(int rowIndex, string columnName)",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}CellText({{0}}, {{1}})\", rowIndex, columnName);",
                $"",
                $"var columnIndex = GetColumnIndex(columnName);",
                $"",
                $"var element = {control.Name}.GetSubElement(driver, By.XPath($\"./tbody/tr[{{rowIndex}}]/td[{{columnIndex}}]\"));",
                $"return element.GetText(driver);",
                $"}}",
                "",
                $"private int GetColumnIndex(string columnName)",
                $"{{",
                $"var columnNames = {control.Name}.GetSubElements(driver, By.XPath(\"./thead/tr/th\"));",
                $"",
                $"int index = columnNames.FindIndex((IWebElement element) => element.Text == columnName);",
                $"",
                $"if (index == -1)",
                $"throw new ApplicationException($\"The column name '{{columnName}}' was not found...\");",
                $"",
                $"return index + 1;",
                $"}}",
                $"",
            };

            return listOfLines;
        }
    }
}
