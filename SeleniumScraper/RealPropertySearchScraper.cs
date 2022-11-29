using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumScraper
{
    public class RealPropertySearchScraper
    {
        private WebDriver WebDriver { get; set; } = null;
        private string DriverPath { get; set; } = @"C:\ChromeDriver\chromedriver.exe";
        private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";

        private WebDriver GetChromeDriver()
        {
            var options = new ChromeOptions();
            options.AddArguments("--headless");
            return new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        }

        [SetUp]
        public void Setup()
        {
            WebDriver = GetChromeDriver();
            WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
        }

        [TearDown]
        public void TearDown()
        {
            WebDriver.Quit();
        }

        [Test]
        public void Scrape()
        {
            // Selecting "BALTIMORE CITY"
            Thread.Sleep(300);
            WebDriver.Navigate().GoToUrl(BaseUrl);
            var input = WebDriver.FindElement(By.XPath("/html/body/form/div[4]/div[4]/div/div/main/div[3]/div/div/table/tbody/tr[1]/td/div/div/fieldset/div[1]/div[2]/select/option[4]"));
            input.Click();

            // Selecting "PROPERTY ACCOUNT IDENTIFIER"
            Thread.Sleep(300);
            input = WebDriver.FindElement(By.XPath("/html/body/form/div[4]/div[4]/div/div/main/div[3]/div/div/table/tbody/tr[1]/td/div/div/fieldset/div[2]/div[2]/select/option[3]"));
            input.Click();

            // Click Continue button
            Thread.Sleep(300);
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue\"]"));
            input.Click();
            Thread.Sleep(3000);

            // Input Ward
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtWard\"]"));
            input.Clear();
            input.SendKeys("01");

            // Input Section
            Thread.Sleep(300);
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtSection\"]"));
            input.Clear();
            input.SendKeys("01");

            // Input Block
            Thread.Sleep(300);
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtBlock\"]"));
            input.Clear();
            input.SendKeys("1738");

            // Input Lot
            Thread.Sleep(300);
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtLot\"]"));
            input.Clear();
            input.SendKeys("001");

            // Click Next button
            Thread.Sleep(300);
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton\"]"));
            input.Click();
            Thread.Sleep(3000);

            // Click Ground Rent Registration link
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0\"]"));
            input.Click();
            Thread.Sleep(3000);

            // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
            input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr\"]"));
            if (input != null)
            {
                if (input.Text.Contains("There is currently no ground rent"))
                {
                    // Property is fee simple
                    // property.IsGroundRent == false;
                }
            }
            // Property must be registered as ground rent
            // property.IsGroundRent == true;
        }
    }
}