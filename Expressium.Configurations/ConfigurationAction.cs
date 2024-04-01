using System;
using System.Linq;

namespace Expressium.Configurations
{
    public class ConfigurationAction
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string How { get; set; }
        public string Using { get; set; }
        public string Value { get; set; }

        public ConfigurationAction()
        {
        }

        public override int GetHashCode()
        {
            return string.Concat(Name, Type, How, Using).GetHashCode();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("The EnrollerAction property 'Name' is undefined...");

            if (!Enum.GetNames(typeof(ControlTypes)).Any(e => Type == e))
                throw new ArgumentException("The EnrollerAction property 'Type' is invalid...");

            if (!Enum.GetNames(typeof(ControlHows)).Any(e => How == e))
                throw new ArgumentException("The EnrollerAction property 'How' is invalid...");

            if (string.IsNullOrWhiteSpace(Using))
                throw new ArgumentException("The EnrollerAction property 'Using' is undefined...");
        }
    }
}
