using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Expressium.CodeGenerators
{
    public static class CodeGeneratorLoaders
    {
        public static ICodeGenerator GetCodeGenerator(Configuration configuration, ObjectRepository objectRepository)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var language = configuration.CodeGenerator.CodingLanguage;
            var dllPath = Path.Combine(currentDirectory, $"Expressium.CodeGenerators.{language}.dll");
            var className = $"Expressium.CodeGenerators.{language}.CodeGenerator";

            var assembly = Assembly.LoadFrom(dllPath);
            var type = assembly.GetType(className);

            object[] constructorArgs = { configuration, objectRepository };
            object instance = Activator.CreateInstance(type, constructorArgs);
            if (instance is ICodeGenerator codeGenerator)
                return codeGenerator;

            throw new ApplicationException("No matching Code Generator was found...");
        }

        public static List<string> GetCodeGeneratorsCodingLanguages()
        {
            var languages = new List<string>();

            var currentDirectory = Directory.GetCurrentDirectory();

            var listOfDlls = Directory.GetFiles(currentDirectory, $"Expressium.CodeGenerators.*.dll", SearchOption.TopDirectoryOnly);
            foreach (var dllPath in listOfDlls)
            {
                var fileName = Path.GetFileName(dllPath);
                var language = fileName.Replace("Expressium.CodeGenerators.", "").Replace(".dll", "");
                var className = $"Expressium.CodeGenerators.{language}.CodeGenerator";

                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    var type = assembly.GetType(className);

                    object instance = Activator.CreateInstance(type);
                    if (instance is ICodeGenerator codeGenerator)
                        languages.AddRange(codeGenerator.GetCodingLanguages());
                }
                catch 
                {
                }
            }

            return languages;
        }

        public static List<string> GetCodeGeneratorsCodingFlavours(string language)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dllPath = Path.Combine(currentDirectory, $"Expressium.CodeGenerators.{language}.dll");
            var className = $"Expressium.CodeGenerators.{language}.CodeGenerator";

            var assembly = Assembly.LoadFrom(dllPath);
            var type = assembly.GetType(className);

            object instance = Activator.CreateInstance(type);
            if (instance is ICodeGenerator codeGenerator)
                return codeGenerator.GetCodingFlavours();

            throw new ApplicationException("No matching Code Generator was found...");
        }

        public static List<string> GetCodeGeneratorsCodingStyles(string language)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var dllPath = Path.Combine(currentDirectory, $"Expressium.CodeGenerators.{language}.dll");
            var className = $"Expressium.CodeGenerators.{language}.CodeGenerator";

            var assembly = Assembly.LoadFrom(dllPath);
            var type = assembly.GetType(className);

            object instance = Activator.CreateInstance(type);
            if (instance is ICodeGenerator codeGenerator)
                return codeGenerator.GetCodingStyles();

            throw new ApplicationException("No matching Code Generator was found...");
        }
    }
}
