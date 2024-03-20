using System;

namespace Expressium.ObjectRepositories
{
    public class ObjectRepositoryMember
    {
        public string Name { get; set; }
        public string Page { get; set; }

        public ObjectRepositoryMember()
        {
        }

        public ObjectRepositoryMember Copy()
        {
            var copy = new ObjectRepositoryMember();
            copy.Assign(this);
            return copy;
        }

        public void Assign(ObjectRepositoryMember member)
        {
            Name = member.Name;
            Page = member.Page;
        }

        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                throw new ArgumentException(string.Format("The ObjectRepositoryMember property 'Name' is undefined..."));

            if (string.IsNullOrWhiteSpace(Page))
                throw new ArgumentException(string.Format("The ObjectRepositoryMember property 'Page' is undefined..."));
        }

        public override int GetHashCode()
        {
            return string.Concat(Name).GetHashCode();
        }
    }
}