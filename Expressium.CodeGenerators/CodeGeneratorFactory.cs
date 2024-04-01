using Expressium.Configurations;
using Expressium.ObjectRepositories;
using System.Collections.Generic;

namespace Expressium.CodeGenerators
{
    internal abstract class CodeGeneratorFactory : CodeGeneratorObject
    {
        internal CodeGeneratorFactory(Configuration configuration, ObjectRepository objectRepository) : base(configuration, objectRepository)
        {
        }

        internal override void Generate(ObjectRepositoryPage page)
        {
            var filePath = GetSourceCodeFilePath(page);
            if (!IsTestFileModified(filePath))
            {
                var sourceCode = GenerateSourceCode(page);
                var listOfLines = FormatSourceCode(sourceCode);
                SaveListOfLinesAsFile(filePath, listOfLines);
            }
        }

        internal override string GeneratePreview(ObjectRepositoryPage page)
        {
            var sourceCode = GenerateSourceCode(page);
            var listOfLines = FormatSourceCode(sourceCode);
            return GetListOfLinesAsString(listOfLines);
        }

        internal abstract string GetSourceCodeFilePath(ObjectRepositoryPage page);
        internal abstract List<string> GenerateSourceCode(ObjectRepositoryPage page);
    }
}
