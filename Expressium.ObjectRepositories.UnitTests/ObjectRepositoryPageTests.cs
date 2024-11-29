using NUnit.Framework;
using Expressium.ObjectRepositories;

namespace Expressium.ObjectRepositories.UnitTests
{
    [TestFixture]
    public class ObjectRepositoryPageTests
    {
        [Test]
        public void ObjectRepositoryPage_Copy()
        {
            var page = new ObjectRepositoryPage();
            CreatePage(page);

            page = page.Copy();

            Assert.That(2, Is.EqualTo(page.Synchronizers.Count), "ObjectRepositoryPage synchronizers validation");
            Assert.That(2, Is.EqualTo(page.Members.Count), "ObjectRepositoryPage members validation");
            Assert.That(2, Is.EqualTo(page.Controls.Count), "ObjectRepositoryPage controls validation");
        }

        private void CreatePage(ObjectRepositoryPage page)
        {
            var synchronizer = new ObjectRepositorySynchronizer() { How = "WaitForPageTitleEquals", Using = "Home" };
            page.AddSynchronizer(synchronizer);
            var synchronizerCopy = page.GetSynchronizer(synchronizer).Copy();
            synchronizerCopy.How = "WaitForPageTitleContains";
            page.AddSynchronizer(synchronizerCopy);

            var member = new ObjectRepositoryMember() { Name = "Menu", Page = "MainMenuBar" };
            page.AddMember(member);
            var memberCopy = page.GetMember(member).Copy();
            memberCopy.Name = "SideBar";
            page.AddMember(memberCopy);

            var control = new ObjectRepositoryControl() { Name = "Username", Type = "TextBox", How = "Id", Using = "username", Target = "ProductPage" };
            page.AddControl(control);
            var controlCopy = page.GetControl(control).Copy();
            controlCopy.Name = "Identifier";
            page.AddControl(controlCopy);
        }
    }
}
