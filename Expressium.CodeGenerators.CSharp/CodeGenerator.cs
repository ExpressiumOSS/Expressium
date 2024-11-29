using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;
using System.Collections.Generic;

namespace Expressium.CodeGenerators.CSharp
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly Configuration configuration;
        private readonly ObjectRepository objectRepository;

        private readonly CodeGeneratorPage codeGeneratorPage;
        private readonly CodeGeneratorModel codeGeneratorModel;
        private readonly CodeGeneratorTest codeGeneratorTest;
        private readonly CodeGeneratorFactory codeGeneratorFactory;
        private readonly CodeGeneratorSolution codeGeneratorSolution;

        public CodeGenerator()
        {
        }

        public CodeGenerator(Configuration configuration, ObjectRepository objectRepository)
        {
            configuration.Validate();
            objectRepository.Validate();

            this.configuration = configuration;
            this.objectRepository = objectRepository;

            codeGeneratorPage = new CodeGeneratorPage(configuration, objectRepository);
            codeGeneratorModel = new CodeGeneratorModel(configuration, objectRepository);
            codeGeneratorTest = new CodeGeneratorTest(configuration, objectRepository);
            codeGeneratorFactory = new CodeGeneratorFactory(configuration, objectRepository);
            codeGeneratorSolution = new CodeGeneratorSolution(configuration);
        }

        public void GenerateAll()
        {
            foreach (var page in objectRepository.Pages)
            {
                codeGeneratorPage.Generate(page);
                if (page.Model)
                    codeGeneratorModel.Generate(page);

                codeGeneratorTest.Generate(page);
                if (page.Model)
                    codeGeneratorFactory.Generate(page);
            }
        }

        public void GeneratePage(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);

                codeGeneratorPage.Generate(page);
                if (page.Model)
                    codeGeneratorModel.Generate(page);

                codeGeneratorTest.Generate(page);
                if (page.Model)
                    codeGeneratorFactory.Generate(page);
            }
        }

        public string GeneratePagePreview(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                return codeGeneratorPage.GeneratePreview(page);
            }

            return null;
        }

        public string GenerateModelPreview(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                if (page.Model)
                    return codeGeneratorModel.GeneratePreview(page);
            }

            return null;
        }

        public string GenerateTestPreview(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                return codeGeneratorTest.GeneratePreview(page);
            }

            return null;
        }

        public string GenerateFactoryPreview(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                if (page.Model)
                    return codeGeneratorFactory.GeneratePreview(page);
            }

            return null;
        }

        public void GenerateSolution()
        {
            codeGeneratorSolution.GenerateAll();
        }

        public List<string> GetCodingLanguages()
        {
            return new List<string>(Enum.GetNames(typeof(CodingLanguages)));
        }

        public List<string> GetCodingFlavours()
        {
            return new List<string>(Enum.GetNames(typeof(CodingFlavours)));
        }

        public List<string> GetCodingStyles()
        {
            return new List<string>(Enum.GetNames(typeof(CodingStyles)));
        }
    }
}
