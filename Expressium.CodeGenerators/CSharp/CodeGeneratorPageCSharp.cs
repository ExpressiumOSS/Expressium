using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.CSharp
{
    internal class CodeGeneratorPageCSharp : CodeGeneratorPage
    {
        internal CodeGeneratorPageCSharp(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override string GetSourceCodeFilePath(ObjectRepositoryPage page)
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
            listOfLines.AddRange(GenerateExtensionSourceCode(page));
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
            {
                listOfLines.Add($"public {member.Page} {member.Name} {{ get; private set; }}");
                listOfLines.Add("");
            }

            return listOfLines;
        }

        internal List<string> GenerateLocators(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            if (configuration.IsCodingStyleByLocators())
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(CodeGeneratorControlCSharp.GenerateByLocator(control));

                if (listOfLines.Count > 0)
                    listOfLines.Add("");
            }
            else
            {
                foreach (var control in page.Controls)
                    listOfLines.AddRange(CodeGeneratorControlCSharp.GenerateFindsByLocator(control));
            }

            return listOfLines;
        }

        internal List<string> GenerateContructor(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            listOfLines.Add($"public {page.Name}(ILog logger, IWebDriver driver) : base(logger, driver)");
            listOfLines.Add($"{{");

            if (page.Members.Count > 0)
            {
                foreach (var member in page.Members)
                    listOfLines.Add($"{member.Name} = new {member.Page}(logger, driver);");
            }

            if (page.Members.Count > 0 && page.Synchronizers.Count > 0)
                listOfLines.Add("");

            if (page.Synchronizers.Count > 0)
            {
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
            }

            listOfLines.Add($"}}");
            listOfLines.Add($"");

            return listOfLines;
        }

        internal List<string> GenerateControlMethods(ObjectRepositoryPage page)
        {
            var listOfLines = new List<string>();

            foreach (var control in page.Controls)
                listOfLines.AddRange(CodeGeneratorControlCSharp.GenerateMethod(control));

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

        internal List<string> GenerateExtensionSourceCode(ObjectRepositoryPage page)
        {
            if (configuration.IncludeExtensions)
            {
                var filePath = GetSourceCodeFilePath(page);
                return GenerateExtensionCode(filePath, "#region Extensions", "#endregion");
            }

            return new List<string>();
        }
    }
}
