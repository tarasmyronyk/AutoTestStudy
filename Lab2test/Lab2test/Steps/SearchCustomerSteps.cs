using NUnit.Framework;
using OpenQA.Selenium;
using TechTalk.SpecFlow;
using YourProjectName.Pages;
namespace YourProjectName.Steps
{
    [Binding]
    public class SearchCustomerSteps
    {
        private readonly IWebDriver _driver;
        private readonly LoginPage _loginPage;
        private readonly ManagerPage _managerPage;

        public SearchCustomerSteps(ScenarioContext scenarioContext)
        {
            // Дістаємо наш драйвер з контексту та ініціалізуємо сторінки
            _driver = (IWebDriver)scenarioContext["WebDriver"];
            _loginPage = new LoginPage(_driver);
            _managerPage = new ManagerPage(_driver);
        }

        [Given(@"I navigate to the banking project website")]
        public void GivenINavigateToTheBankingProjectWebsite()
        {
            _loginPage.GoToUrl("https://www.globalsqa.com/angularJs-protractor/BankingProject/#/login");
        }

        [When(@"I click on the Bank Manager Login button")]
        public void WhenIClickOnTheBankManagerLoginButton()
        {
            _loginPage.ClickManagerLogin();
        }

        [When(@"I go to the Customers tab")]
        public void WhenIGoToTheCustomersTab()
        {
            _managerPage.GoToCustomersTab();
        }

        [When(@"I search for customer ""(.*)""")]
        public void WhenISearchForCustomer(string name)
        {
            _managerPage.SearchForCustomer(name);
        }

        [Then(@"I should see customer ""(.*)"" in the search results")]
        public void ThenIShouldSeeCustomerInTheSearchResults(string expectedName)
        {
            string actualName = _managerPage.GetFirstCustomerNameInTable();
            // Assert перевіряє, чи очікуваний результат співпадає з реальним
            Assert.That(actualName, Is.EqualTo(expectedName));
        }
    }
}