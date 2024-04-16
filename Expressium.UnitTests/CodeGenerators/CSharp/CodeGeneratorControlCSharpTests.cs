using NUnit.Framework;
using Expressium.CodeGenerators.CSharp;
using Expressium.ObjectRepositories;

namespace Expressium.UnitTests.CodeGenerators.CSharp
{
    [TestFixture]
    public class CodeGeneratorControlCSharpTests
    {
        [Test]
        public void CodeGeneratorControlCSharp_GenerateFindsByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = CodeGeneratorControlCSharp.GenerateFindsByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(3), "CodeGeneratorControlCSharp GenerateFindsByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("[FindsBy(How = How.Id, Using = \"search\")]"), "CodeGeneratorControlCSharp GenerateFindsByLocator validation");
            Assert.That(listOfLines[1], Is.EqualTo("private IWebElement Search { get; set; }"), "CodeGeneratorControlCSharp GenerateFindsByLocator validation");
            Assert.That(listOfLines[2], Is.EqualTo(""), "CodeGeneratorControlCSharp GenerateFindsByLocator validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateByLocator()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.How = "Id";
            control.Using = "search";

            var listOfLines = CodeGeneratorControlCSharp.GenerateByLocator(control);

            Assert.That(listOfLines.Count, Is.EqualTo(1), "CodeGeneratorControlCSharp GenerateByLocator validation");
            Assert.That(listOfLines[0], Is.EqualTo("private readonly By Search = By.Id(\"search\");"), "CodeGeneratorControlCSharp GenerateByLocator validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_TextBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Search";
            control.Type = "TextBox";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetSearch(string value)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Search.SetTextBox(driver, value);"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetSearch()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Search.GetTextBox(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_RadioButton()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Yes";
            control.Type = "RadioButton";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetYes(bool value)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Yes.SetRadioButton(driver, value);"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetYes()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Yes.GetRadioButton(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_CheckBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Agreed";
            control.Type = "CheckBox";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetAgreed(bool value)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Agreed.SetCheckBox(driver, value);"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public bool GetAgreed()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Agreed.GetCheckBox(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_ComboBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ComboBox";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetProduct(string value)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetComboBox(driver, value);"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetComboBox(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_ListBox()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Product";
            control.Type = "ListBox";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(12), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void SetProduct(string value)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Product.SetListBox(driver, value);"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public string GetProduct()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Product.GetListBox(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_Link()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "AboutUs";
            control.Type = "Link";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void ClickAboutUs()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("AboutUs.ClickLink(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_Button()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Submit";
            control.Type = "Button";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public void ClickSubmit()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("Submit.ClickButton(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_Text()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Heading";
            control.Type = "Text";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(6), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public string GetHeading()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return Heading.GetText(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        [Test]
        public void CodeGeneratorControlCSharp_GenerateMethod_Table_Without_Headers()
        {
            var control = new ObjectRepositoryControl();
            control.Name = "Events";
            control.Type = "Table";

            var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

            Assert.That(listOfLines.Count, Is.EqualTo(48), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[0], Is.EqualTo("public int GetEventsNumberOfRows()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[3], Is.EqualTo("return Events.GetSubElements(driver, By.XPath(\"./tbody/tr\")).Count;"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[6], Is.EqualTo("public int GetEventsNumberOfColumns()"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[9], Is.EqualTo("return Events.GetSubElements(driver, By.XPath(\"./thead/tr/th\")).Count;"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[12], Is.EqualTo("public void ClickEventsCell(int rowIndex, int columnIndex)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[15], Is.EqualTo("var element = Events.GetSubElement(driver, By.XPath($\"./tbody/tr[{rowIndex}]/td[{columnIndex}]\"));"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[16], Is.EqualTo("element.Click(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[19], Is.EqualTo("public string GetEventsCellText(int rowIndex, int columnIndex)"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[22], Is.EqualTo("var element = Events.GetSubElement(driver, By.XPath($\"./tbody/tr[{rowIndex}]/td[{columnIndex}]\"));"), "CodeGeneratorControlCSharp GenerateMethod validation");
            Assert.That(listOfLines[23], Is.EqualTo("return element.GetText(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        }

        //[Test]
        //public void CodeGeneratorControlCSharp_GenerateMethod_Table_With_Headers()
        //{
        //    var control = new ObjectRepositoryControl();
        //    control.Name = "Events";
        //    control.Type = "Table";
        //    control.Value = "#Id;Description;Price;";

        //    var listOfLines = CodeGeneratorControlCSharp.GenerateMethod(control);

        //    Assert.That(listOfLines.Count, Is.EqualTo(37), "CodeGeneratorControlCSharp GenerateMethod validation");
        //    Assert.That(listOfLines[0], Is.EqualTo("private enum EventsColumns"), "CodeGeneratorControlCSharp GenerateMethod validation");
        //    Assert.That(listOfLines[7], Is.EqualTo("public int GetNumberOfEvents()"), "CodeGeneratorControlCSharp GenerateMethod validation");
        //    Assert.That(listOfLines[13], Is.EqualTo("public string GetEventsIdText(int rowIndex)"), "CodeGeneratorControlCSharp GenerateMethod validation");
        //    Assert.That(listOfLines[16], Is.EqualTo("return GetEventsText(rowIndex, (int)EventsColumns.Id);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        //    Assert.That(listOfLines[33], Is.EqualTo("var element = Events.GetSubElement(driver, By.XPath($\"./tbody/tr[{rowIndex}]/td[position()={columnIndex}]\"));"), "CodeGeneratorControlCSharp GenerateMethod validation");
        //    Assert.That(listOfLines[34], Is.EqualTo("return element.GetText(driver);"), "CodeGeneratorControlCSharp GenerateMethod validation");
        //}
    }
}
