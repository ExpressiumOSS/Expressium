using System;
using System.Linq;

namespace Expressium.Configurations
{
    public class ConfigurationEnrollerAction
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string How { get; set; }
        public string Using { get; set; }
        public string Value { get; set; }

        public ConfigurationEnrollerAction()
        {
        }

        public override int GetHashCode()
        {
            return string.Concat(Name, Type, How, Using).GetHashCode();
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException("The ConfigurationEnrollerAction property 'Name' is undefined...");

            if (string.IsNullOrWhiteSpace(Type))
                throw new ArgumentException("The ConfigurationEnrollerAction property 'Type' is undefined...");

            if (string.IsNullOrWhiteSpace(How))
                throw new ArgumentException("The ConfigurationEnrollerAction property 'How' is undefined...");

            if (string.IsNullOrWhiteSpace(Using))
                throw new ArgumentException("The ConfigurationEnrollerAction property 'Using' is undefined...");
        }
    }
}
