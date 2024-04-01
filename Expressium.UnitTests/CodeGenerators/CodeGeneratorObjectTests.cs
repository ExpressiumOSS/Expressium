using NUnit.Framework;
using Expressium.CodeGenerators;
using System.Collections.Generic;

namespace Expressium.UnitTests.CodeGenerators
{
    [TestFixture]
    public class CodeGeneratorObjectTests
    {
        [Test]
        public void CodeGeneratorObject_FormatSourceCode()
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

            var result = CodeGeneratorObject.FormatSourceCode(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorObject FormatSourceCode validate generated output");
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
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorObject GetListOfLinesAsString validate generated output");
        }

        [Test]
        public void ConfigurationObject_GenerateExtensionCode()
        {
            var listOfLines = CodeGeneratorObject.GenerateExtensionCode(null, "#region Extensions", "#endregion");

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorObject GenerateExtensionCode validation");
        }
    }
}
