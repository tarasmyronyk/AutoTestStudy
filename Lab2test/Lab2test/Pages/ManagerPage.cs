using OpenQA.Selenium;
using System.Threading;

namespace YourProjectName.Pages
{
    public class ManagerPage
    {
        private readonly IWebDriver _driver;

        public ManagerPage(IWebDriver driver)
        {
            _driver = driver;
        }

        private IWebElement CustomersTab => _driver.FindElement(By.XPath("//button[contains(text(), 'Customers')]"));
        private IWebElement SearchInput => _driver.FindElement(By.XPath("//input[@ng-model='searchCustomer']"));
        private IWebElement FirstResultFirstName => _driver.FindElement(By.XPath("//table/tbody/tr[1]/td[1]"));

        public void GoToCustomersTab()
        {
            // Невелика затримка для промальовування елементів Angular (в ідеалі використовувати WebDriverWait)
            Thread.Sleep(1000);
            CustomersTab.Click();
        }

        public void SearchForCustomer(string name)
        {
            Thread.Sleep(1000);
            SearchInput.Clear();
            SearchInput.SendKeys(name);
        }

        public string GetFirstCustomerNameInTable()
        {
            Thread.Sleep(1000);
            return FirstResultFirstName.Text;
        }
    }
}
