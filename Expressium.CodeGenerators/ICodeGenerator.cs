using System;
using System.Collections.Generic;

namespace Expressium.CodeGenerators
{
    public interface ICodeGenerator
    {
        public void GenerateAll();

        public void GeneratePage(string name);

        public string GeneratePagePreview(string name);
        public string GenerateModelPreview(string name);
        public string GenerateTestPreview(string name);
        public string GenerateFactoryPreview(string name);

        public void GenerateSolution();

        public List<string> GetCodingLanguages();
        public List<string> GetCodingFlavours();
    }
}
