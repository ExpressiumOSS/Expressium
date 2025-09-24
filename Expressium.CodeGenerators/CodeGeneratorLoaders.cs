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
            var flavour = configuration.CodeGenerator.CodingFlavour;
            var dllPath = Path.Combine(currentDirectory, $"Expressium.CodeGenerators.{language}.{flavour}.dll");
            var className = $"Expressium.CodeGenerators.{language}.{flavour}.CodeGenerator";

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

                var tokens = fileName.Replace("Expressium.CodeGenerators.", "").Replace(".dll", "").Split('.');
                if (tokens.Length != 2)
                    continue;

                var language = tokens[0];
                var flavour = tokens[1];

                var className = $"Expressium.CodeGenerators.{language}.{flavour}.CodeGenerator";

                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    var type = assembly.GetType(className);

                    object instance = Activator.CreateInstance(type);
                    if (instance is ICodeGenerator codeGenerator)
                    {
                        if (!languages.Contains(language))
                            languages.Add(language);
                    }
                }
                catch
                {
                }
            }

            return languages;
        }

        public static List<string> GetCodeGeneratorsCodingFlavours(string language)
        {
            var flavours = new List<string>();

            var currentDirectory = Directory.GetCurrentDirectory();

            var listOfDlls = Directory.GetFiles(currentDirectory, $"Expressium.CodeGenerators.{language}.*.dll", SearchOption.TopDirectoryOnly);
            foreach (var dllPath in listOfDlls)
            {
                var fileName = Path.GetFileName(dllPath);
                var flavour = fileName.Replace($"Expressium.CodeGenerators.{language}.", "").Replace(".dll", "");
                var className = $"Expressium.CodeGenerators.{language}.{flavour}.CodeGenerator";

                try
                {
                    var assembly = Assembly.LoadFrom(dllPath);
                    var type = assembly.GetType(className);

                    object instance = Activator.CreateInstance(type);
                    if (instance is ICodeGenerator codeGenerator)
                    {
                        if (!flavours.Contains(flavour))
                            flavours.Add(flavour);
                    }
                }
                catch
                {
                }
            }

            return flavours;
        }
    }
}
