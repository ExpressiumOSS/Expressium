using NUnit.Framework;
using Expressium.ObjectRepositories;
using System;

namespace Expressium.ObjectRepositories.UnitTests
{
    [TestFixture]
    public class ObjectRepositoryControlTests
    {
        [Test]
        public void ObjectRepositoryControl_Validate_Undefined_Name()
        {
            var control = new ObjectRepositoryControl();
            control.Name = null;
            control.Type = "TextBox";
            control.How = "XPath";
            control.Using = "//a[text)='Home']";

            var exception = Assert.Throws<ArgumentException>(() => control.Validate());
            Assert.That(exception.Message, Is.EqualTo("The ObjectRepositoryControl property 'Name' is undefined..."), "ObjectRepositoryControl Validate undefine property Name");
        }

        [Test]
        public void ObjectRepositoryControl_Validate_Valid_Type()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Home";
            control.Type = "ComboBoxPrimefaces";
            control.How = "XPath";
            control.Using = "//a[text)='Home']";

            Assert.That(control.IsComboBox, Is.True, "ObjectRepositoryControl Validate valid property Type");
            Assert.DoesNotThrow(() => control.Validate(), "ObjectRepositoryControl Validate valid property Type");
        }

        [Test]
        public void ObjectRepositoryControl_Validate_Invalid_Type()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Home";
            control.Type = "TestBoks";
            control.How = "XPath";
            control.Using = "//a[text)='Home']";

            var exception = Assert.Throws<ArgumentException>(() => control.Validate());
            Assert.That(exception.Message, Is.EqualTo("The ObjectRepositoryControl property 'Type' is invalid..."), "ObjectRepositoryControl Validate invalid property Type");
        }

        [Test]
        public void ObjectRepositoryControl_Validate_Invalid_How()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Home";
            control.Type = "TextBox";
            control.How = "TPath";
            control.Using = "//a[text)='Home']";

            var exception = Assert.Throws<ArgumentException>(() => control.Validate());
            Assert.That(exception.Message, Is.EqualTo("The ObjectRepositoryControl property 'How' is invalid..."), "ObjectRepositoryControl Validate invalid property How");
        }

        [Test]
        public void ObjectRepositoryControl_Validate_Undefined_Using()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Homw";
            control.Type = "TextBox";
            control.How = "XPath";
            control.Using = "";

            var exception = Assert.Throws<ArgumentException>(() => control.Validate());
            Assert.That(exception.Message, Is.EqualTo("The ObjectRepositoryControl property 'Using' is undefined..."), "ObjectRepositoryControl Validate undefine property Using");
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase("Link", false)]
        [TestCase("Element", true)]
        public void ObjectRepositoryControl_IsElement(string input, bool expected)
        {
            var control = new ObjectRepositoryControl() { Type = input };
            Assert.That(expected, Is.EqualTo(control.IsElement()), "ObjectRepositoryControl IsElement validation");
        }
    }
}
