using NUnit.Framework;
using System.Collections.Generic;

namespace Expressium.CodeGenerators.UnitTests
{
    [TestFixture]
    public class CodeGeneratorUtilitiesTests
    {
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("www.google.com", false)]
        [TestCase("https://www.google.com", true)]
        [TestCase("http://www.google.com", true)]
        public void CodeGeneratorUtilities_IsValidURI(string input, bool expected)
        {
            Assert.That(expected, Is.EqualTo(CodeGeneratorUtilities.IsValidURI(input)), "CodeGeneratorUtilities IsValidURI validate URI");
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("1Class", false)]
        [TestCase("HomePage", true)]
        [TestCase("@LoginPage", false)]
        [TestCase("My Page", false)]
        [TestCase("My#Page", false)]
        [TestCase("My\"Page", false)]
        [TestCase("OnePage2", true)]
        [TestCase(".OnePage", false)]
        [TestCase("OnePage.", false)]
        [TestCase("OnePage.Ib", false)]
        [TestCase("onePage", true)]
        [TestCase("one_Page", true)]
        [TestCase(" onePage", false)]
        [TestCase(" onePage ", false)]
        [TestCase("_one_Page", false)]
        public void CodeGeneratorUtilities_IsValidCSharpClass(string input, bool expected)
        {
            Assert.That(expected, Is.EqualTo(CodeGeneratorUtilities.IsValidClassName(input)), "CodeGeneratorUtilities IsValidCSharpClass validate class name");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("a", "a")]
        [TestCase("abc", "abc")]
        [TestCase("Abc", "abc")]
        [TestCase("ABC", "aBC")]
        public void CodeGeneratorUtilities_CamelCasing(string input, string expected)
        {
            string result = input.CamelCase();
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorUtilities CamelCasing validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("a", "A")]
        [TestCase("abc", "Abc")]
        [TestCase("Abc", "Abc")]
        [TestCase("ABC", "ABC")]
        public void CodeGeneratorUtilities_PascalCasing(string input, string expected)
        {
            string result = input.PascalCase();
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorUtilities PascalCasing validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("hello   world", "helloworld")]
        [TestCase(" hello my world ", "hellomyworld")]
        public void CodeGeneratorUtilities_RemoveWhiteSpaces(string input, string expected)
        {
            Assert.That(expected, Is.EqualTo(input.RemoveWhiteSpaces()), "CodeGeneratorUtilities RemoveWhiteSpaces validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("hello   world", "hello world")]
        [TestCase("hello 'my' world", "hello my world")]
        [TestCase("h_ello world", "h ello world")]
        [TestCase("hello's world", "hellos world")]
        public void CodeGeneratorUtilities_RemoveIllegalCharacters(string input, string expected)
        {
            Assert.That(expected, Is.EqualTo(input.RemoveIllegalCharacters()), "CodeGeneratorUtilities RemoveIllegalCharacters validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("hello world", "Hello World")]
        [TestCase("hEllo worlD", "HEllo WorlD")]
        public void CodeGeneratorUtilities_CapitalizeWords(string input, string expected)
        {
            Assert.That(expected, Is.EqualTo(input.CapitalizeWords()), "CodeGeneratorUtilities CapitalizeWords validate generated output");
        }

        [Test]
        public void CodeGeneratorUtilities_FormatSourceCode()
        {
            var input = new List<string>
            {
                "public class AssetsBar : BasePage",
                "{",
                "public bool AssetsBar(bool value)",
                "{",
                "if (value)",
                "return true;",
                "else if (!value)",
                "return false;",
                "else ",
                "return false;",
                "",
                "if (x)",
                "if (y)",
                "return true;",
                "",
                "while (a==b)",
                "continue;",
                "",
                "for (int i=0; i<count; i++)",
                "bool x = true;",
                "}",
                "}"
            };

            var expected = new List<string>
            {
                "public class AssetsBar : BasePage",
                "{",
                "    public bool AssetsBar(bool value)",
                "    {",
                "        if (value)",
                "            return true;",
                "        else if (!value)",
                "            return false;",
                "        else",
                "            return false;",
                "",
                "        if (x)",
                "            if (y)",
                "                return true;",
                "",
                "        while (a==b)",
                "            continue;",
                "",
                "        for (int i=0; i<count; i++)",
                "            bool x = true;",
                "    }",
                "}"
            };

            var result = CodeGeneratorUtilities.FormatSourceCode(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorUtilities FormatSourceCode validation");
        }
    }
}
