using OpenQA.Selenium;

namespace YourProjectName.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        // Знаходимо кнопку логіну менеджера
        private IWebElement ManagerLoginBtn => _driver.FindElement(By.XPath("//button[contains(text(), 'Bank Manager Login')]"));

        public void GoToUrl(string url)
        {
            _driver.Navigate().GoToUrl(url);
        }

        public void ClickManagerLogin()
        {
            ManagerLoginBtn.Click();
        }
    }
}
