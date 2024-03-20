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

    public enum EnrollerActionTypes
    {
        Element,
        Link,
        Button,
        TextBox,
        ComboBox,
        ListBox,
        RadioButton,
        CheckBox,
        Heading,
        Table
    }

    public enum EnrollerActionHows
    {
        Id,
        Name,
        ClassName,
        CssSelector,
        XPath,
        LinkText,
        PartialLinkText
    }
}
