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
            if (!string.IsNullOrWhiteSpace(control.Value))
                return GenerateTableWithHeaderMethods(control);
            else
                return GenerateTableWithoutHeaderMethods(control);
        }

        internal static List<string> GenerateTableWithHeaderMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>();

            var listOfHeaders = control.Value.Split(';');
            var listOfUniqueHeaders = new List<string>();

            listOfLines.Add($"private enum {control.Name}Columns");
            listOfLines.Add($"{{");

            for (int i = 0; i < listOfHeaders.Count(); i++)
            {
                var header = listOfHeaders[i].Trim();
                header = header.PascalCase();

                if (listOfUniqueHeaders.Contains(header))
                    continue;

                if (string.IsNullOrWhiteSpace(header))
                    continue;

                listOfUniqueHeaders.Add(header);

                listOfLines.Add($"{header} = {i + 1},");
            }

            listOfLines.Add($"}}");
            listOfLines.Add("");

            listOfLines.AddRange(new List<string>
            {
                $"public int GetNumberOf{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"GetNumberOf{control.Name}()\");",
                $"return {control.Name}.GetChildWebElements(driver, By.XPath(\"./tbody/tr\")).Count;",
                $"}}",
                ""
            });

            foreach (var header in listOfUniqueHeaders)
            {
                listOfLines.AddRange(new List<string>
                {
                    $"public string Get{control.Name}{header}Text(int rowIndex)",
                    $"{{",
                    $"logger.InfoFormat(\"Get{control.Name}{header}Text({{0}})\", rowIndex);",
                    $"return Get{control.Name}Text(rowIndex, (int){control.Name}Columns.{header});",
                    $"}}",
                    ""
                });
            }

            listOfLines.AddRange(new List<string>
            {
                $"private string Get{control.Name}Text(int rowIndex, int columnIndex)",
                $"{{",
                $"var element = {control.Name}.GetChildWebElement(driver, By.XPath($\"./tbody/tr[{{rowIndex}}]/td[position()={{columnIndex}}]\"));",
                $"return element.GetText(driver);",
                $"}}",
                ""
            });

            return listOfLines;
        }

        internal static List<string> GenerateTableWithoutHeaderMethods(ObjectRepositoryControl control)
        {
            var listOfLines = new List<string>
            {
                $"public int GetNumberOf{control.Name}()",
                $"{{",
                $"logger.InfoFormat(\"GetNumberOf{control.Name}()\");",
                $"return {control.Name}.GetChildWebElements(driver, By.XPath(\"./tbody/tr\")).Count;",
                $"}}",
                "",
                $"public void Click{control.Name}(int rowId, int columnId)",
                $"{{",
                $"logger.InfoFormat(\"Click{control.Name}({{0}}, {{1}})\", rowId, columnId);",
                $"var element = {control.Name}.GetChildWebElement(driver, By.XPath($\"./tbody/tr[{{rowId}}]/td[position()={{columnId}}]//a\"));",
                $"element.Click(driver);",
                $"}}",
                "",
                $"public string Get{control.Name}Text(int rowIndex, int columnIndex)",
                $"{{",
                $"logger.InfoFormat(\"Get{control.Name}Text({{0}}, {{1}})\", rowIndex, columnIndex);",
                $"var element = {control.Name}.GetChildWebElement(driver, By.XPath($\"./tbody/tr[{{rowIndex}}]/td[position()={{columnIndex}}]\"));",
                $"return element.GetText(driver);",
                $"}}",
                ""
            };

            return listOfLines;
        }
    }
}
