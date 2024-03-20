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
                throw new ArgumentException(string.Format("The EnrollerAction property 'Name' is undefined..."));

            if (!Enum.GetNames(typeof(EnrollerActionTypes)).Any(e => Type == e))
                throw new ArgumentException(string.Format("The EnrollerAction property 'Type' is invalid..."));

            if (!Enum.GetNames(typeof(EnrollerActionHows)).Any(e => How == e))
                throw new ArgumentException(string.Format("The EnrollerAction property 'How' is invalid..."));

            if (string.IsNullOrWhiteSpace(Using))
                throw new ArgumentException(string.Format("The EnrollerAction property 'Using' is undefined..."));
        }
    }
}
