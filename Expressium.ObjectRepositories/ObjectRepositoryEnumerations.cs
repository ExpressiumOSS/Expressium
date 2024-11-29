using System;

namespace Expressium.ObjectRepositories
{
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
}
