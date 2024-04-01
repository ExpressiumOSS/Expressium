using NUnit.Framework;
using Expressium.CodeGenerators.CSharp;
using Expressium.Configurations;
using Expressium.ObjectRepositories;

namespace Expressium.UnitTests.CodeGenerators.CSharp
{
    [TestFixture]
    public class CodeGeneratorModelCSharpTests
    {
        private Configuration configuration;
        private ObjectRepository objectRepository;
        private ObjectRepositoryPage page;

        private CodeGeneratorModelCSharp codeGeneratorModelCSharp;

        [OneTimeSetUp]
        public void Setup()
        {
            configuration = new Configuration();
            configuration.Company = "Expressium";
            configuration.Project = "Coffeeshop";

            page = new ObjectRepositoryPage();
            page.Name = "RegistrationPage";
            page.Title = "Registration";
            page.Model = true;
            page.Controls.Add(new ObjectRepositoryControl() { Name = "FirstName", Type = ControlTypes.TextBox.ToString(), How = ControlHows.Name.ToString(), Using = "firstname" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "LastName", Type = ControlTypes.TextBox.ToString(), How = ControlHows.Name.ToString(), Using = "lastname" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Country", Type = ControlTypes.ComboBox.ToString(), How = ControlHows.Name.ToString(), Using = "country", Value = "Denmark" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Male", Type = ControlTypes.RadioButton.ToString(), How = ControlHows.Id.ToString(), Using = "gender_0", Value = "False" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "Female", Type = ControlTypes.RadioButton.ToString(), How = ControlHows.Id.ToString(), Using = "gender_1", Value = "True" });
            page.Controls.Add(new ObjectRepositoryControl() { Name = "IAgreeToTheTermsOfUse", Type = ControlTypes.CheckBox.ToString(), How = ControlHows.Name.ToString(), Using = "agreement" });

            objectRepository = new ObjectRepository();
            objectRepository.AddPage(page);

            codeGeneratorModelCSharp = new CodeGeneratorModelCSharp(configuration, objectRepository);
        }

        [Test]
        public void CodeGeneratorModelCSharp_GenerateSourceCode()
        {
            var listOfLines = codeGeneratorModelCSharp.GenerateSourceCode(page);

            Assert.That(listOfLines.Count, Is.EqualTo(19), "CodeGeneratorModelCSharp GenerateSourceCode validation");
        }

        [Test]
        public void CodeGeneratorModelCSharp_GenerateUsings()
        {
            var listOfLines = codeGeneratorModelCSharp.GenerateUsings();

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorModelCSharp GenerateUsings validation");
            Assert.That(listOfLines[0], Is.EqualTo("using System;"), "CodeGeneratorModelCSharp GenerateUsings validation");
            Assert.That(listOfLines[1], Is.EqualTo("using System.Collections.Generic;"), "CodeGeneratorModelCSharp GenerateUsings validation");
        }

        [Test]
        public void codeGeneratorModelCSharp_GenerateNameSpace()
        {
            var listOfLines = codeGeneratorModelCSharp.GenerateNameSpace();

            Assert.That(listOfLines.Count, Is.EqualTo(1), "codeGeneratorModelCSharp GenerateNameSpace validation");
            Assert.That(listOfLines[0], Is.EqualTo("namespace Expressium.Coffeeshop.Web.API.Models"), "codeGeneratorModelCSharp GenerateNameSpace validation");
        }

        [Test]
        public void codeGeneratorModelCSharp_GenerateClass()
        {
            var listOfLines = codeGeneratorModelCSharp.GenerateClass(page);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "codeGeneratorModelCSharp GenerateClass validation");
            Assert.That(listOfLines[0], Is.EqualTo("public class RegistrationPageModel"), "codeGeneratorModelCSharp GenerateClass validation");
        }

        [Test]
        public void CodeGeneratorModelCSharp_GenerateProperties()
        {
            var listOfLines = codeGeneratorModelCSharp.GenerateProperties(page);

            Assert.That(listOfLines.Count, Is.EqualTo(7), "CodeGeneratorModelCSharp GenerateProperties validation");
            Assert.That(listOfLines[0], Is.EqualTo("public string FirstName { get; set; }"), "CodeGeneratorModelCSharp GenerateProperties validation");
            Assert.That(listOfLines[1], Is.EqualTo("public string LastName { get; set; }"), "CodeGeneratorModelCSharp GenerateProperties validation");
            Assert.That(listOfLines[2], Is.EqualTo("public string Country { get; set; }"), "CodeGeneratorModelCSharp GenerateProperties validation");
            Assert.That(listOfLines[3], Is.EqualTo("public bool Male { get; set; }"), "CodeGeneratorModelCSharp GenerateProperties validation");
            Assert.That(listOfLines[4], Is.EqualTo("public bool Female { get; set; }"), "CodeGeneratorModelCSharp GenerateProperties validation");
            Assert.That(listOfLines[5], Is.EqualTo("public bool IAgreeToTheTermsOfUse { get; set; }"), "CodeGeneratorModelCSharp GenerateProperties validation");
        }
    }
}
