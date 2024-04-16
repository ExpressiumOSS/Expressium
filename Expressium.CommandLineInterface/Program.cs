using Expressium.CodeGenerators;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using Expressium.SolutionGenerators;
using System;

namespace Expressium.CommandLineInterface
{
    public class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0] == "--solutiongenerator")
            {
                SolutionGenerator(args[1]);
            }
            else if (args.Length == 2 && args[0] == "--codegenerator")
            {
                CodeGenerator(args[1]);
            }
            else
            {
                Console.WriteLine("Expressium.CommandLineInterface.exe [OPTION] [CONFIGURATION]");
                Console.WriteLine("Expressium.CommandLineInterface.exe --solutiongenerator C:\\SourceCode\\company-project-tests\\CompanyProject.cfg");
                Console.WriteLine("Expressium.CommandLineInterface.exe --codegenerator C:\\SourceCode\\company-project-tests\\CompanyProject.cfg");
            }
        }

        internal static void SolutionGenerator(string filePath)
        {
            Console.WriteLine("Expressium Solution Generator...");

            var configuration = ConfigurationUtilities.DeserializeAsJson<Configuration>(filePath);

            var solutionGenerator = new SolutionGenerator(configuration);
            solutionGenerator.GenerateAll();

            Console.WriteLine("Expressium Solution Generator completed");
            Console.WriteLine(" ");
        }

        internal static void CodeGenerator(string filePath)
        {
            Console.WriteLine("Expressium Code Generator...");

            var configuration = ConfigurationUtilities.DeserializeAsJson<Configuration>(filePath);
            var objectRepository = ObjectRepositoryUtilities.DeserializeAsJson<ObjectRepository>(configuration.RepositoryPath);

            var codeGenerator = new CodeGenerator(configuration, objectRepository);
            codeGenerator.GenerateAll();

            Console.WriteLine("Expressium Code Generator completed");
            Console.WriteLine(" ");
        }
    }
}
