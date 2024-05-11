using System;

namespace Expressium.CodeGenerators
{
    public interface ICodeGenerator
    {
        public void GenerateAll();
        public void GeneratePage(string name);
        public string GenerateModelAsString(string name);
        public string GenerateFactoryAsString(string name);
        public string GeneratePageAsString(string name);
        public string GenerateTestAsString(string name);
    }
}
