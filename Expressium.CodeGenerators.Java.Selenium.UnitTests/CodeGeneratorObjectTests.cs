using Expressium.Configurations;
using Expressium.ObjectRepositories;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Expressium.CodeGenerators.Java.Selenium.UnitTests
{
    [TestFixture]
    public class CodeGeneratorObjectTests
    {
        [Test]
        public void Configuration_GetPackage()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            var codeGeneratorObject = new CodeGeneratorObject(configuration, null);
            var result = codeGeneratorObject.GetPackage();
            var expected = "expressium.coffeeshop.web.api";

            Assert.That(result, Is.EqualTo(expected), "Configuration GetPackage validate generated output");
        }

        [Test]
        public void Configuration_GetPackagePath()
        {
            var configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            var codeGeneratorObject = new CodeGeneratorObject(configuration, null);
            var result = codeGeneratorObject.GetPackagePath();
            var expected = "expressium/coffeeshop/web/api";

            Assert.That(result, Is.EqualTo(expected), "Configuration GetPackagePath validate generated output");
        }

        [Test]
        public void CodeGeneratorObject_GetSourceCodeAsFormatted()
        {
            var input = new List<string>
            {
                "public class AssetsBar extends BasePage",
                "{",
                "public boolean assetsBar(boolean value)",
                "{",
                "if (value)",
                "return true;",
                "else if (!value)",
                "return false;",
                "else",
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
                "boolean x = true;",
                "}",
                "}"
            };

            var expected = new List<string>
            {
                "public class AssetsBar extends BasePage",
                "{",
                "    public boolean assetsBar(boolean value)",
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
                "            boolean x = true;",
                "    }",
                "}"
            };

            var result = CodeGeneratorObject.GetSourceCodeAsFormatted(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorObject GetSourceCodeAsFormatted validation");
        }

        [Test]
        public void CodeGeneratorObject_GetSourceCodeAsString()
        {
            var input = new List<string>
            {
                "<Model>",
                "<Attribute name='FaultLocation'>",
                "</Model>"
            };

            var expected = "<Model>\r\n<Attribute name='FaultLocation'>\r\n</Model>";

            var result = CodeGeneratorObject.GetSourceCodeAsString(input);
            Assert.That(result, Is.EqualTo(expected), "CodeGeneratorObject GetSourceCodeAsString validation");
        }

        [Test]
        public void ConfigurationObject_GenerateSourceCodeExtensionMethods()
        {
            var listOfLines = CodeGeneratorObject.GenerateSourceCodeExtensionMethods(null, "// region Extensions", "// endregion");

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorObject GenerateSourceCodeExtensionMethods validation");
        }

        [Test]
        public void ConfigurationObject_GenerateSourceCodeExtensionMethods_With_File()
        {
            var directory = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(directory, "SnippetJava.txt");
            File.Delete(filePath);
            File.WriteAllText(filePath, "// region Extensions\n\n// This is my life...\n\n// endregion\n");

            var listOfLines = CodeGeneratorObject.GenerateSourceCodeExtensionMethods(filePath, "// region Extensions", "// endregion");

            Assert.That(listOfLines.Count, Is.EqualTo(5), "CodeGeneratorObject GenerateSourceCodeExtensionMethods validation");
            Assert.That(listOfLines[0], Is.EqualTo("// region Extensions"), "CodeGeneratorObject GenerateSourceCodeExtensionMethods validation");
            Assert.That(listOfLines[2], Is.EqualTo("// This is my life..."), "CodeGeneratorObject GenerateSourceCodeExtensionMethods validation");
            Assert.That(listOfLines[4], Is.EqualTo("// endregion"), "CodeGeneratorObject GenerateSourceCodeExtensionMethods validation");
        }

        [Test]
        public void CodeGeneratorObject_IsSourceCodeModified_With_Comment()
        {
            var directory = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(directory, "ExportPageJava.txt");

            File.Delete(filePath);
            File.WriteAllText(filePath, "// TODO - Implement...");

            Assert.That(CodeGeneratorObject.IsSourceCodeModified(filePath), Is.False, "CodeGeneratorObject IsSourceCodeModified validation");
        }

        [Test]
        public void CodeGeneratorObject_IsSourceCodeModified_Without_Comment()
        {
            var directory = Environment.GetEnvironmentVariable("TEMP");
            var filePath = Path.Combine(directory, "ImportPageJava.txt");

            File.Delete(filePath);
            File.WriteAllText(filePath, "// TODO - Updated...");

            Assert.That(CodeGeneratorObject.IsSourceCodeModified(filePath), Is.True, "CodeGeneratorObject IsSourceCodeModified validation");
        }
    }
}
