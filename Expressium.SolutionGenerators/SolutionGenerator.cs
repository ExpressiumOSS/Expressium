using Expressium.Configurations;
using Expressium.SolutionGenerators.CSharp;
using Expressium.SolutionGenerators.Java;
using System;

namespace Expressium.SolutionGenerators
{
    public class SolutionGenerator
    {
        private SolutionGeneratorProject solutionGeneratorProject;

        public SolutionGenerator(Configuration configuration)
        {
            configuration.Validate();

            if (configuration.IsCodingLanguageCSharp())
            {
                solutionGeneratorProject = new SolutionGeneratorProjectCSharp(configuration);
            }
            else if (configuration.IsCodingLanguageJava())
            {
                solutionGeneratorProject = new SolutionGeneratorProjectJava(configuration);
            }
            else
            {
                throw new ApplicationException("Unknown CodingLanguage configuration property...");
            }
        }

        public void GenerateAll()
        {
            solutionGeneratorProject.GenerateAll();
        }
    }
}
