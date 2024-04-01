using NUnit.Framework;
using Expressium.ObjectRepositories;

namespace Expressium.UnitTests.ObjectRepositories
{
    [TestFixture]
    public class ObjectRepositoryTests
    {
        [Test]
        public void ObjectRepositoryProject_AddPage()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            Assert.That(objectRepository.IsPageAdded("LoginPage"), Is.True, "ObjectRepository AddPage validation");
            Assert.That("LoginPage", Is.EqualTo(objectRepository.GetPage("LoginPage").Name), "ObjectRepository GetPage validation");
        }

        [Test]
        public void ObjectRepositoryProject_DeletePage()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            objectRepository.DeletePage("LoginPage");
            Assert.That(objectRepository.IsPageAdded("LoginPage"), Is.False, "ObjectRepository DeletePage validation");
        }

        [Test]
        public void ObjectRepositoryProject_EditPage()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            var page = new ObjectRepositoryPage();
            page.Name = "FrontPage";
            page.AddControl(new ObjectRepositoryControl() { Name = "Cancel", Type = "Button", How = "XPath", Using = "//button[@id='cancel']" });
            objectRepository.EditPage("LoginPage", page);
            Assert.That(objectRepository.IsPageAdded("LoginPage"), Is.False, "ObjectRepository EditPage validation");
            Assert.That(objectRepository.IsPageAdded("FrontPage"), Is.True, "ObjectRepository EditPage validation");
            Assert.That(1, Is.EqualTo(objectRepository.GetPage("FrontPage").Controls.Count), "ObjectRepository EditPage validation");
        }

        [Test]
        public void ObjectRepositoryProject_GetNumberOfObjects()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            objectRepository.AddPage(CreateProductPage());
            objectRepository.AddPage(CreateMainMenuBar());

            Assert.That(3, Is.EqualTo(objectRepository.Pages.Count), "ObjectRepository GetNumberOfPages validation");
        }

        [Test]
        public void ObjectRepositoryProject_DeletePageUsage()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            objectRepository.AddPage(CreateMainMenuBar());
            objectRepository.GetPage("LoginPage").Base = "MainMenuBar";
            objectRepository.DeletePage("MainMenuBar");

            Assert.That(objectRepository.IsPageAdded("MainMenuBar"), Is.False, "ObjectRepository IsPageAdded validation");
            Assert.That(objectRepository.GetPage("LoginPage").Base, Is.Null, "ObjectRepository DeletePageUsage validation");
        }

        [Test]
        public void ObjectRepositoryProject_RenamePageUsage()
        {
            var objectRepository = new ObjectRepository();
            objectRepository.AddPage(CreateLoginPage());
            var loginPage = objectRepository.GetPage("LoginPage");
            loginPage.Base = "MainMenuBar";

            objectRepository.AddPage(CreateMainMenuBar());

            var mainMenuBar = objectRepository.GetPage("MainMenuBar").Copy();
            mainMenuBar.Name = "MenuBar";
            objectRepository.EditPage("MainMenuBar", mainMenuBar);

            Assert.That(objectRepository.IsPageAdded("MenuBar"), Is.True, "ObjectRepository IsPageAdded validation");
            Assert.That("MenuBar", Is.EqualTo(objectRepository.GetPage("LoginPage").Base), "ObjectRepository RenamePageUsage validation");
        }

        private ObjectRepositoryPage CreateLoginPage()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "LoginPage";

            var username = new ObjectRepositoryControl();
            username.Name = "Username";
            username.Type = "TextBox";
            username.How = "Id";
            username.Using = "username";
            page.AddControl(username);

            var password = new ObjectRepositoryControl();
            password.Name = "Password";
            password.Type = "TextBox";
            password.How = "Name";
            password.Using = "password";
            page.AddControl(password);

            var submit = new ObjectRepositoryControl();
            submit.Name = "LogIn";
            submit.Type = "Button";
            submit.How = "XPath";
            submit.Using = "//button[text()='LogIn']";
            page.AddControl(submit);

            page.Model = true;

            return page;
        }

        private ObjectRepositoryPage CreateProductPage()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "ProducPage";

            var username = new ObjectRepositoryControl();
            username.Name = "Heading";
            username.Type = "Text";
            username.How = "XPath";
            username.Using = "//h1[text()']";
            page.AddControl(username);

            return page;
        }

        private ObjectRepositoryPage CreateMainMenuBar()
        {
            var page = new ObjectRepositoryPage();
            page.Name = "MainMenuBar";

            var home = new ObjectRepositoryControl();
            home.Name = "Home";
            home.Type = "Link";
            home.How = "XPath";
            home.Using = "//a[text()='Home']";
            page.AddControl(home);

            var product = new ObjectRepositoryControl();
            product.Name = "Home";
            product.Type = "Link";
            product.How = "XPath";
            product.Using = "//a[text()='Product']";
            page.AddControl(product);

            return page;
        }
    }
}
