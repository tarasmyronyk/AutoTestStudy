using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;

namespace YourProjectName.Hooks
{
    [Binding]
    public sealed class TestInitialize
    {
        private readonly ScenarioContext _scenarioContext;

        public TestInitialize(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            // Зберігаємо драйвер у контексті сценарію, щоб передати його сторінкам
            _scenarioContext["WebDriver"] = driver;
        }

        [AfterScenario]
        public void AfterScenario()
        {
            var driver = (IWebDriver)_scenarioContext["WebDriver"];
            driver.Quit();
            driver.Dispose();
        }
    }
}
