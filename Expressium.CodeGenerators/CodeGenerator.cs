using Expressium.CodeGenerators.CSharp;
using Expressium.CodeGenerators.Java;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;

namespace Expressium.CodeGenerators
{
    public class CodeGenerator : ICodeGenerator
    {
        private readonly Configuration configuration;
        private readonly ObjectRepository objectRepository;

        private readonly CodeGeneratorPage codeGeneratorPage;
        private readonly CodeGeneratorModel codeGeneratorModel;
        private readonly CodeGeneratorTest codeGeneratorTest;
        private readonly CodeGeneratorFactory codeGeneratorFactory;

        public CodeGenerator(Configuration configuration, ObjectRepository objectRepository)
        {
            configuration.Validate();
            objectRepository.Validate();

            this.configuration = configuration;
            this.objectRepository = objectRepository;

            if (configuration.IsCodingLanguageCSharp())
            {
                codeGeneratorPage = new CodeGeneratorPageCSharp(configuration, objectRepository);
                codeGeneratorModel = new CodeGeneratorModelCSharp(configuration, objectRepository);
                codeGeneratorTest = new CodeGeneratorTestCSharp(configuration, objectRepository);
                codeGeneratorFactory = new CodeGeneratorFactoryCSharp(configuration, objectRepository);
            }
            else if (configuration.IsCodingLanguageJava())
            {
                codeGeneratorPage = new CodeGeneratorPageJava(configuration, objectRepository);
                codeGeneratorModel = new CodeGeneratorModelJava(configuration, objectRepository);
                codeGeneratorTest = new CodeGeneratorTestJava(configuration, objectRepository);
                codeGeneratorFactory = new CodeGeneratorFactoryJava(configuration, objectRepository);
            }
            else
            {
                throw new ApplicationException("Unknown CodingLanguage configuration property...");
            }
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

        public string GeneratePageAsString(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                return codeGeneratorPage.GenerateAsString(page);
            }

            return null;
        }

        public string GenerateTestAsString(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                return codeGeneratorTest.GenerateAsString(page);
            }

            return null;
        }

        public string GenerateModelAsString(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                if (page.Model)
                    return codeGeneratorModel.GenerateAsString(page);
            }

            return null;
        }

        public string GenerateFactoryAsString(string name)
        {
            if (objectRepository.IsPageAdded(name))
            {
                var page = objectRepository.GetPage(name);
                if (page.Model)
                    return codeGeneratorFactory.GenerateAsString(page);
            }

            return null;
        }
    }
}
