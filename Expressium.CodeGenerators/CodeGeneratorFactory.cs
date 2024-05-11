using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;

namespace Expressium.CodeGenerators
{
    internal abstract class CodeGeneratorFactory : BaseCodeGenerator
    {
        internal CodeGeneratorFactory(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal abstract string GetFilePath(ObjectRepositoryPage page);
        internal abstract List<string> GenerateSourceCode(ObjectRepositoryPage page);
        internal abstract bool IsFileModified(string filePath);

        internal override void Generate(ObjectRepositoryPage page)
        {
            var filePath = GetFilePath(page);
            if (!IsFileModified(filePath))
            {
                var sourceCode = GenerateSourceCode(page);
                var listOfLines = FormatSourceCode(sourceCode);
                SaveSourceCode(filePath, listOfLines);
            }
        }

        internal override string GenerateAsString(ObjectRepositoryPage page)
        {
            var sourceCode = GenerateSourceCode(page);
            var listOfLines = FormatSourceCode(sourceCode);
            return GetSourceCodeAsString(listOfLines);
        }
    }
}
