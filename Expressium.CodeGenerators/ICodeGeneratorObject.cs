using Expressium.ObjectRepositories;

namespace Expressium.CodeGenerators
{
    public interface ICodeGeneratorObject
    {
        public void Generate(ObjectRepositoryPage page);
        public string GeneratePreview(ObjectRepositoryPage page);
    }
}
