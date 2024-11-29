using Expressium.Configurations;
using System;
using System.Linq;

namespace Expressium.ObjectRepositories
{
    public class ObjectRepositoryControl
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string How { get; set; }
        public string Using { get; set; }
        public string Source { get; set; }
        public string Target { get; set; }
        public string Value { get; set; }

        public ObjectRepositoryControl()
        {
        }

        public ObjectRepositoryControl Copy()
        {
            var copy = new ObjectRepositoryControl();
            copy.Assign(this);
            return copy;
        }

        public void Assign(ObjectRepositoryControl control)
        {
            Name = control.Name;
            Type = control.Type;
            How = control.How;
            Using = control.Using;
            Target = control.Target;
            Value = control.Value;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("The ObjectRepositoryControl property 'Name' is undefined...");

            if (!Enum.GetNames(typeof(ControlTypes)).Any(e => Type.StartsWith(e)))
                throw new ArgumentException("The ObjectRepositoryControl property 'Type' is invalid...");

            if (!Enum.GetNames(typeof(ControlHows)).Any(e => How == e))
                throw new ArgumentException("The ObjectRepositoryControl property 'How' is invalid...");

            if (string.IsNullOrWhiteSpace(Using))
                throw new ArgumentException("The ObjectRepositoryControl property 'Using' is undefined...");
        }

        public override int GetHashCode()
        {
            return string.Concat(Name, Type, How, Using).GetHashCode();
        }

        public override string ToString()
        {
            return $"Name = {Name}, Type = {Type}, How = {How}, Using = {Using}";
        }

        public bool IsElement()
        {
            if (Type == ControlTypes.Element.ToString())
                return true;

            return false;
        }

        public bool IsLink()
        {
            if (Type.StartsWith(ControlTypes.Link.ToString()))
                return true;

            return false;
        }

        public bool IsButton()
        {
            if (Type.StartsWith(ControlTypes.Button.ToString()))
                return true;

            return false;
        }

        public bool IsCheckBox()
        {
            if (Type.StartsWith(ControlTypes.CheckBox.ToString()))
                return true;

            return false;
        }

        public bool IsRadioButton()
        {
            if (Type.StartsWith(ControlTypes.RadioButton.ToString()))
                return true;

            return false;
        }

        public bool IsComboBox()
        {
            if (Type.StartsWith(ControlTypes.ComboBox.ToString()))
                return true;

            return false;
        }

        public bool IsListBox()
        {
            if (Type.StartsWith(ControlTypes.ListBox.ToString()))
                return true;

            return false;
        }

        public bool IsTextBox()
        {
            if (Type.StartsWith(ControlTypes.TextBox.ToString()))
                return true;

            return false;
        }

        public bool IsText()
        {
            if (Type == ControlTypes.Text.ToString())
                return true;

            return false;
        }

        public bool IsTable()
        {
            if (Type == ControlTypes.Table.ToString())
                return true;

            return false;
        }

        public bool IsFillFormControl()
        {
            if (IsTextBox())
                return true;
            else if (IsComboBox())
                return true;
            else if (IsListBox())
                return true;
            else if (IsCheckBox())
                return true;
            else if (IsRadioButton())
                return true;
            else
            {
            }

            return false;
        }
    }
}
