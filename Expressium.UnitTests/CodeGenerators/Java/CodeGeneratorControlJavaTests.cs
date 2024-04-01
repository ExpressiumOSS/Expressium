using Expressium.CodeGenerators.Java;
using Expressium.ObjectRepositories;
using NUnit.Framework;

namespace Expressium.UnitTests.CodeGenerators.Java
{
    [TestFixture]
    public class CodeGeneratorControlJavaTests
    {
        [TestCase("Id", "ID")]
        [TestCase("Name", "NAME")]
        [TestCase("ClassName", "CLASS_NAME")]
        [TestCase("CssSelector", "CSS")]
        [TestCase("XPath", "XPATH")]
        [TestCase("LinkText", "LINK_TEXT")]
        [TestCase("PartialLinkText", "PARTIAL_LINK_TEXT")]
        [TestCase("TagName", "TAG_NAME")]
        public void CodeGeneratorControlJava_GetFindbysHowMapping(string input, string expected)
        {
            var value = CodeGeneratorControlJava.GetFindbysHowMapping(input);
            Assert.That(value, Is.EqualTo(expected), "CodeGeneratorControlJava GetFindbysHowMapping validate mapping...");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateFindsByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = CodeGeneratorControlJava.GenerateFindsByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorControlJava GenerateFindsByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("@FindBy(how = How.ID, using = \"search\")"), "CodeGeneratorControlJava GenerateFindsByLocator validation");
            Assert.That(listOfLines[1], Is.EqualTo("private WebElement search;"), "CodeGeneratorControlJava GenerateFindsByLocator validation");
            Assert.That(listOfLines[2], Is.EqualTo(""), "CodeGeneratorControlJava GenerateFindsByLocator validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = CodeGeneratorControlJava.GenerateByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorControlJava GenerateByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private By search = By.id(\"search\");"), "CodeGeneratorControlJava GenerateByLocator validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setSearch(String value) throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setTextBox(driver, search, value);"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getSearch() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getTextBox(driver, search);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setYes(boolean value) throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setRadioButton(driver, yes, value);"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public boolean getYes() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getRadioButton(driver, yes);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setAgreed(boolean value) throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setCheckBox(driver, agreed, value);"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public boolean getAgreed() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getCheckBox(driver, agreed);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setProduct(String value) throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setComboBox(driver, product, value);"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getProduct() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getComboBox(driver, product);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void setProduct(String value) throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.setListBox(driver, product, value);"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public String getProduct() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return WebElements.getListBox(driver, product);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void clickAboutUs() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.clickLink(driver, aboutUs);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void clickSubmit() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("WebElements.clickButton(driver, submit);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public String getHeading() throws Exception"), "CodeGeneratorControlJava GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return WebElements.getText(driver, heading);"), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_Table_Without_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorControlJava GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlJava_GenerateMethod_Table_With_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";
            control.Value = "Id;Description;Price";

            var listOfLines = CodeGeneratorControlJava.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(0), "CodeGeneratorControlJava GenerateMethod validation");
        }
    }
}
