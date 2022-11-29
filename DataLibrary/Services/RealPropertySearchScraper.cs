using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace DataLibrary.Services;

public class RealPropertySearchScraper : IRealPropertySearchScraper
{
    private readonly DataContext _dataContext;
    private readonly IAddressDataServiceFactory _addressDataServiceFactory;
    private WebDriver WebDriver { get; set; } = null;
    private string DriverPath { get; set; } = @"C:\ChromeDriver\chromedriver.exe";
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    public RealPropertySearchScraper(
        DataContext dataContext,
        IAddressDataServiceFactory addressDataServiceFactory)
    {
        _dataContext = dataContext;
        _addressDataServiceFactory = addressDataServiceFactory;
        var options = new ChromeOptions();
        options.AddArguments("--headless");
        WebDriver = new ChromeDriver(DriverPath, options, TimeSpan.FromSeconds(300));
        WebDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(120);
    }
    public async Task Scrape(List<AddressModel> addressList)
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

        foreach (var address in addressList)
        {
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
            bool result;
            if (input != null)
            {
                if (input.Text.Contains("There is currently no ground rent"))
                {
                    // Property is not ground rent
                    address.IsGroundRent = false;
                    using (var uow = _dataContext.CreateUnitOfWork())
                    {
                        var addressDataService = _addressDataServiceFactory.CreateAddressDataService(uow);
                        result = await addressDataService.CreateOrUpdateFromSDATIsGroundRent(new AddressModel
                        {
                            AccountId = address.AccountId,
                            IsGroundRent = address.IsGroundRent
                        });
                    }
                    if (result is true)
                    {
                        // Go to previous page
                        Thread.Sleep(300);
                        input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_FinishNavigationTemplateContainerID_StepPreviousButton\"]"));
                        input.Click();
                        Thread.Sleep(3000);

                        // Go to second previous page leading to Ward, Section, Block, and Lot input
                        input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepPreviousButton\"]"));
                        input.Click();
                        Thread.Sleep(3000);
                    }
                    else if (result is false)
                    {
                        // Something wrong happened and I do not want the application to skip over this address
                        WebDriver.Quit();
                        Console.WriteLine($"Could not complete transaction for {address.AccountId}");
                    }
                }
            }
            // Property must be ground rent
            address.IsGroundRent = true;
            using (var uow = _dataContext.CreateUnitOfWork())
            {
                var addressDataService = _addressDataServiceFactory.CreateAddressDataService(uow);
                result = await addressDataService.CreateOrUpdateFromSDATIsGroundRent(new AddressModel
                {
                    AccountId = address.AccountId,
                    IsGroundRent = address.IsGroundRent
                });
            }
            if (result is true)
            {
                // Go to previous page
                Thread.Sleep(300);
                input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_FinishNavigationTemplateContainerID_StepPreviousButton\"]"));
                input.Click();
                Thread.Sleep(3000);

                // Go to second previous page leading to Ward, Section, Block, and Lot input
                input = WebDriver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepPreviousButton\"]"));
                input.Click();
                Thread.Sleep(3000);
            }
            else if (result is false)
            {
                // Something wrong happened and I do not want the application to skip over this address
                WebDriver.Quit();
                Console.WriteLine($"Could not complete transaction for {address.AccountId}");
            }
        }
        // addressList has been iterated through
        WebDriver.Quit();
    }
}
