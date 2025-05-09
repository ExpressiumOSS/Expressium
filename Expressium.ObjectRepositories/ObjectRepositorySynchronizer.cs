using System;
using System.Linq;

namespace Expressium.ObjectRepositories
{
    public class ObjectRepositorySynchronizer
    {
        public string How { get; set; }
        public string Using { get; set; }

        public ObjectRepositorySynchronizer()
        {
        }

        public ObjectRepositorySynchronizer Copy()
        {
            var copy = new ObjectRepositorySynchronizer();
            copy.Assign(this);
            return copy;
        }

        public void Assign(ObjectRepositorySynchronizer synchronizer)
        {
            How = synchronizer.How;
            Using = synchronizer.Using;
        }

        public void Validate()
        {
            if (!Enum.GetNames(typeof(SynchronizerTypes)).Any(e => How == e))
                throw new ArgumentException("The ObjectRepositorySynchronizer property 'How' is invalid...");

            if (string.IsNullOrWhiteSpace(Using))
                throw new ArgumentException("The ObjectRepositorySynchronizer property 'Using' is undefined...");
        }

        public override int GetHashCode()
        {
            return string.Concat(How, Using).GetHashCode();
        }
    }
}