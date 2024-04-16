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
            listOfLines.AddRange(GenerateExtensions(page));
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
            {
                listOfLines.Add($"private {member.Page} {member.Name};");
                listOfLines.Add("");
            }

            return listOfLines;
        }

        internal List<string> GenerateLocators(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
                listOfLines.AddRange(CodeGeneratorControlJava.GenerateFindsByLocator(control));

            return listOfLines;
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
                listOfLines.AddRange(CodeGeneratorControlJava.GenerateMemberMethod(member));

            return listOfLines;
        }

        internal List<string> GenerateControlMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
                listOfLines.AddRange(CodeGeneratorControlJava.GenerateMethod(control));

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

        internal List<string> GenerateExtensions(ObjectRepositoryPage page)
        {
            if (configuration.IncludeExtensions)
            {
                var filePath = GetFilePath(page);
                return GenerateExtensions(filePath, "// region Extensions", "// endregion");
            }

            return new List<string>();
        }
    }
}
