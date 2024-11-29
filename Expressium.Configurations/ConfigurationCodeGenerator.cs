using System;

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
            if (string.IsNullOrWhiteSpace(CodingLanguage))
                throw new ArgumentException("The ConfigurationCodeGenerator property 'CodingLanguage' is undefined...");

            if (string.IsNullOrWhiteSpace(CodingFlavour))
                throw new ArgumentException("The ConfigurationCodeGenerator property 'CodingFlavour' is undefined...");

            if (string.IsNullOrWhiteSpace(CodingStyle))
                throw new ArgumentException("The ConfigurationCodeGenerator property 'CodingStyle' is undefined...");
        }
    }
}
