﻿using Expressium.CodeGenerators.CSharp;
using Expressium.CodeGenerators.Java;
using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System;

namespace Expressium.CodeGenerators
{
    public class CodeGenerator
    {
        private readonly Configuration configuration;
        private readonly ObjectRepository objectRepository;

        private readonly CodeGeneratorModel codeGeneratorModel;
        private readonly CodeGeneratorPage codeGeneratorPage;
        private readonly CodeGeneratorFactory codeGeneratorFactory;
        private readonly CodeGeneratorTest codeGeneratorTest;

        public CodeGenerator(Configuration configuration, ObjectRepository objectRepository)
        {
            configuration.Validate();
            objectRepository.Validate();

            this.configuration = configuration;
            this.objectRepository = objectRepository;

            if (configuration.IsCodingLanguageCSharp())
            {
                codeGeneratorModel = new CodeGeneratorModelCSharp(configuration, objectRepository);
                codeGeneratorPage = new CodeGeneratorPageCSharp(configuration, objectRepository);
                codeGeneratorFactory = new CodeGeneratorFactoryCSharp(configuration, objectRepository);
                codeGeneratorTest = new CodeGeneratorTestCSharp(configuration, objectRepository);
            }
            else if (configuration.IsCodingLanguageJava())
            {
                codeGeneratorModel = new CodeGeneratorModelJava(configuration, objectRepository);
                codeGeneratorPage = new CodeGeneratorPageJava(configuration, objectRepository);
                codeGeneratorFactory = new CodeGeneratorFactoryJava(configuration, objectRepository);
                codeGeneratorTest = new CodeGeneratorTestJava(configuration, objectRepository);
            }
            else
            {
                throw new ApplicationException("Unknown CodingLanguage configuration property...");
            }
        }

        public void GenerateAll()
        {
            if (codeGeneratorPage != null && configuration.IncludePages)
            {
                foreach (var page in objectRepository.Pages)
                {
                    codeGeneratorPage.Generate(page);
                    if (page.Model)
                    {
                        if (codeGeneratorModel != null)
                            codeGeneratorModel.Generate(page);
                    }
                }
            }

            if (codeGeneratorTest != null && configuration.IncludeTests)
            {
                foreach (var page in objectRepository.Pages)
                {
                    codeGeneratorTest.Generate(page);
                    if (page.Model)
                    {
                        if (codeGeneratorFactory != null)
                            codeGeneratorFactory.Generate(page);
                    }
                }
            }
        }

        public void GeneratePage(string name)
        {
            if (codeGeneratorPage != null && configuration.IncludePages)
            {
                if (objectRepository.IsPageAdded(name))
                {
                    var page = objectRepository.GetPage(name);
                    codeGeneratorPage.Generate(page);
                    if (page.Model)
                    {
                        if (codeGeneratorModel != null)
                            codeGeneratorModel.Generate(page);
                    }
                }
            }

            if (codeGeneratorTest != null && configuration.IncludeTests)
            {
                if (objectRepository.IsPageAdded(name))
                {
                    var page = objectRepository.GetPage(name);
                    codeGeneratorTest.Generate(page);
                    if (page.Model)
                    {
                        if (codeGeneratorFactory != null)
                            codeGeneratorFactory.Generate(page);
                    }
                }
            }
        }

        public string GeneratePagePreview(string name)
        {
            if (codeGeneratorPage != null && configuration.IncludePages)
            {
                if (objectRepository.IsPageAdded(name))
                {
                    var page = objectRepository.GetPage(name);
                    return codeGeneratorPage.GeneratePreview(page);
                }
            }

            return null;
        }

        public string GenerateTestPreview(string name)
        {
            if (codeGeneratorTest != null && configuration.IncludeTests)
            {
                if (objectRepository.IsPageAdded(name))
                {
                    var page = objectRepository.GetPage(name);
                    return codeGeneratorTest.GeneratePreview(page);
                }
            }

            return null;
        }

        public string GenerateModelPreview(string name)
        {
            if (codeGeneratorModel != null && configuration.IncludePages)
            {
                if (objectRepository.IsPageAdded(name))
                {
                    var page = objectRepository.GetPage(name);
                    if (page.Model)
                        return codeGeneratorModel.GeneratePreview(page);
                }
            }

            return null;
        }

        public string GenerateFactoryPreview(string name)
        {
            if (codeGeneratorFactory != null && configuration.IncludeTests)
            {
                if (objectRepository.IsPageAdded(name))
                {
                    var page = objectRepository.GetPage(name);
                    if (page.Model)
                        return codeGeneratorFactory.GeneratePreview(page);
                }
            }

            return null;
        }
    }
}