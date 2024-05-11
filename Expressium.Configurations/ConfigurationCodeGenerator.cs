using System;
using System.Linq;

namespace Expressium.Configurations
{
    public class ConfigurationCodeGenerator
    {
        public string CodingLanguage { get; set; }
        public string CodingFlavour { get; set; }
        public string CodingStyle { get; set; }
        public string CustomControlTypes { get; set; }
        public string InitialLoginPage { get; set; }

        public ConfigurationCodeGenerator()
        {
            CustomControlTypes = null;
            InitialLoginPage = null;
        }

        public void Validate()
        {
            if (!Enum.GetNames(typeof(CodingLanguages)).Any(e => CodingLanguage == e))
                throw new ArgumentException("The Configuration property 'CodingLanguage' is invalid...");

            if (!Enum.GetNames(typeof(CodingFlavours)).Any(e => CodingFlavour == e))
                throw new ArgumentException("The Configuration property 'CodingFlavour' is invalid...");

            if (!Enum.GetNames(typeof(CodingStyles)).Any(e => CodingStyle == e))
                throw new ArgumentException("The Configuration property 'CodingStyle' is invalid...");
        }
    }
}
