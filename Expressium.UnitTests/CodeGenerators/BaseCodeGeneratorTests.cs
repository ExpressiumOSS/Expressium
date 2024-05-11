using NUnit.Framework;
using Expressium.CodeGenerators;
using System.Collections.Generic;
using System.IO;
using System;

namespace Expressium.UnitTests.CodeGenerators
{
    [TestFixture]
    public class BaseCodeGeneratorTests
    {
        [Test]
        public void BaseCodeGenerator_FormatSourceCode()
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

            var result = BaseCodeGenerator.FormatSourceCode(input);
            Assert.That(result, Is.EqualTo(expected), "BaseCodeGenerator FormatSourceCode validation");
        }

        [Test]
        public void BaseCodeGenerator_GetSourceCodeAsString()
        {
            var input = new List<string>
            {
                "<Model>",
                "<Attribute name='FaultLocation'>",
                "</Model>"
            };

            var expected = "<Model>\r\n<Attribute name='FaultLocation'>\r\n</Model>";

            var result = BaseCodeGenerator.GetSourceCodeAsString(input);
            Assert.That(result, Is.EqualTo(expected), "BaseCodeGenerator GetSourceCodeAsString validation");
        }

        [Test]
        public void ConfigurationObject_GenerateSourceCodeExtensionMethods()
        {
            var listOfLines = BaseCodeGenerator.GenerateSourceCodeExtensionMethods(null, "#region Extensions", "#endregion");

            Assert.That(listOfLines.Count, Is.EqualTo(3), "BaseCodeGenerator GenerateSourceCodeExtensionMethods validation");
        }

        [Test]
        public void ConfigurationObject_GenerateSourceCodeExtensionMethods_With_File()
        {
            var directory = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(directory, "Snippet.txt");
            File.Delete(filePath);
            File.WriteAllText(filePath, "#region Extensions\n\n// This is my life...\n\n#endregion\n");

            var listOfLines = BaseCodeGenerator.GenerateSourceCodeExtensionMethods(filePath, "#region Extensions", "#endregion");

            Assert.That(listOfLines.Count, Is.EqualTo(5), "BaseCodeGenerator GenerateSourceCodeExtensionMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("#region Extensions"), "BaseCodeGenerator GenerateSourceCodeExtensionMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("// This is my life..."), "BaseCodeGenerator GenerateSourceCodeExtensionMethods validation");
            Assert.That(listOfLines[4], Is.EqualTo("#endregion"), "BaseCodeGenerator GenerateGenerateSourceCodeExtensionMethodsExtensionMethods validation");
        }

        [Test]
        public void BaseCodeGenerator_IsSourceCodeTextInFile()
        {
            var directory = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(directory, "Export.txt");

            File.Delete(filePath);
            File.WriteAllText(filePath, "Hugoline was here...");

            Assert.That(BaseCodeGenerator.IsSourceCodeTextInFile(filePath, "Hugoline was here..."), Is.True, "BaseCodeGenerator IsSourceCodeTextInFile validation");
            Assert.That(BaseCodeGenerator.IsSourceCodeTextInFile(filePath, "Superman was here..."), Is.False, "BaseCodeGenerator IsSourceCodeTextInFile validation");
        }
    }
}
