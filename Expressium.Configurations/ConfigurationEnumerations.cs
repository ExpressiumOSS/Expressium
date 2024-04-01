using System;

namespace Expressium.Configurations
{
    public enum CodingLanguages
    {
        CSharp,
        Java
    }

    public enum CodingFlavours
    {
        Specflow,
        Reqnroll,
        Cucumber
    }

    public enum CodingStyles
    {
        PageFactory,
        ByLocators
    }

    public enum ControlTypes
    {
        Element,
        Link,
        Button,
        TextBox,
        ComboBox,
        ListBox,
        RadioButton,
        CheckBox,
        Text,
        Table
    }

    public enum ControlHows
    {
        Id,
        Name,
        ClassName,
        CssSelector,
        XPath,
        LinkText,
        PartialLinkText,
        TagName
    }

    public enum SynchronizerTypes
    {
        WaitForPageTitleEquals,
        WaitForPageTitleContains,
        WaitForPageUrlEquals,
        WaitForPageUrlContains,
        WaitForPageElementIsVisible,
        WaitForPageElementIsEnabled
    }

    public enum BrowserTypes
    {
        Chrome,
        Edge,
        Firefox
    }

    public enum CodeFolders
    {
        Models,
        Pages
    }

    public enum TestFolders
    {
        Factories,
        UITests,
        BusinessTests
    }
}
