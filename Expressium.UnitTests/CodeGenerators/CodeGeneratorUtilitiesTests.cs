using NUnit.Framework;
using Expressium.CodeGenerators;

namespace Expressium.UnitTests.CodeGenerators
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
            string result = CodeGeneratorUtilities.CamelCase(input);
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
            string result = CodeGeneratorUtilities.PascalCase(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorUtilities PascalCasing validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("hello   world", "helloworld")]
        [TestCase(" hello my world ", "hellomyworld")]
        public void CodeGeneratorUtilities_RemoveWhiteSpaces(string input, string expected)
        {
            Assert.That(expected, Is.EqualTo(CodeGeneratorUtilities.RemoveWhiteSpaces(input)), "CodeGeneratorUtilities RemoveWhiteSpaces validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("hello   world", "hello world")]
        [TestCase("hello 'my' world", "hello my world")]
        [TestCase("h_ello world", "h ello world")]
        [TestCase("hello's world", "hellos world")]
        public void CodeGeneratorUtilities_RemoveIllegalCharacters(string input, string expected)
        {
            Assert.That(expected, Is.EqualTo(CodeGeneratorUtilities.RemoveIllegalCharacters(input)), "CodeGeneratorUtilities RemoveIllegalCharacters validate generated output");
        }

        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("hello world", "Hello World")]
        [TestCase("hEllo worlD", "HEllo WorlD")]
        public void CodeGeneratorUtilities_CapitalizeWords(string input, string expected)
        {
            Assert.That(expected, Is.EqualTo(CodeGeneratorUtilities.CapitalizeWords(input)), "CodeGeneratorUtilities CapitalizeWords validate generated output");
        }
    }
}
