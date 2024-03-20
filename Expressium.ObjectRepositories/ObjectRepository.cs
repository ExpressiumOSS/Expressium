using System.Collections.Generic;
using System.Linq;

namespace Expressium.ObjectRepositories
{
    public class ObjectRepository
    {
        public List<ObjectRepositoryPage> Pages { get; set; }

        public void Validate()
        {
            foreach (var page in Pages)
                page.Validate();
        }

        public ObjectRepository()
        {
            Pages = new List<ObjectRepositoryPage>();
        }

        public bool IsPageAdded(string value)
        {
            return Pages.Any(m => m.Name == value);
        }

        public void AddPage(ObjectRepositoryPage page)
        {
            if (!IsPageAdded(page.Name))
                Pages.Add(page);
        }

        public ObjectRepositoryPage GetPage(string name)
        {
            if (IsPageAdded(name))
                return Pages.Find(x => x.Name == name);

            return null;
        }

        public void EditPage(string name, ObjectRepositoryPage page)
        {
            if (IsPageAdded(name))
            {
                if (name != page.Name)
                {
                    foreach (var _page in Pages)
                        _page.RenameUsage(name, page.Name);
                }

                var item = GetPage(name);
                item.Assign(page);
            }
        }

        public void DeletePage(string name)
        {
            if (IsPageAdded(name))
            {
                foreach (var page in Pages)
                    page.DeleteUsage(name);

                var item = Pages.Find(x => x.Name == name);
                Pages.Remove(item);
            }
        }
    }
}
