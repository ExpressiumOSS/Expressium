using NUnit.Framework;
using Expressium.CodeGenerators;
using System.Collections.Generic;

namespace Expressium.UnitTests.CodeGenerators
{
    [TestFixture]
    public class CodeGeneratorObjectTests
    {
        [Test]
        public void CodeGeneratorObject_GetListOfLinesAsFormatted()
        {
            var input = new List<string>
            {
                "public class AssetsBar : BasePage",
                "{",
                "public AssetsBar(bool value)",
                "{",
                "if (value)",
                "return true;",
                "}",
                "}"
            };

            var expected = new List<string>
            {
                "public class AssetsBar : BasePage",
                "{",
                "    public AssetsBar(bool value)",
                "    {",
                "        if (value)",
                "            return true;",
                "    }",
                "}"
            };

            var result = CodeGeneratorObject.GetListOfLinesAsFormatted(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorObject GetListOfLinesAsFormatted validation");
        }

        [Test]
        public void CodeGeneratorObject_GetListOfLinesAsString()
        {
            var input = new List<string>
            {
                "<Model>",
                "<Attribute name='FaultLocation'>",
                "</Model>"
            };

            var expected = "<Model>\n<Attribute name='FaultLocation'>\n</Model>\n";

            var result = CodeGeneratorObject.GetListOfLinesAsString(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorObject GetListOfLinesAsString validation");
        }

        [Test]
        public void ConfigurationObject_GenerateExtensions()
        {
            var listOfLines = CodeGeneratorObject.GenerateExtensions(null, "#region Extensions", "#endregion");

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorObject GenerateExtensions validation");
        }
    }
}
