using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class MontgomeryCountyScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly MontgomeryCountyDataServiceFactory _montgomeryCountyDataServiceFactory;
    private WebDriver FirefoxDriver { get; set; } = null;
    private IWebElement FirefoxInput { get; set; }
    private string FirefoxDriverPath { get; set; } = @"C:\WebDrivers\geckodriver.exe";
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private int currentCount;
    private int totalCount;
    private decimal percentComplete;
    private int amountToScrape = 3;
    private bool isRestartingScrape = false;
    private List<AddressModel> AddressList = new();

    public MontgomeryCountyScraper(
        IDataContext dataContext,
        MontgomeryCountyDataServiceFactory montgomeryCountyDataServiceFactory)
    {
        _dataContext = dataContext;
        _montgomeryCountyDataServiceFactory = montgomeryCountyDataServiceFactory;

        FirefoxProfile firefoxProfile = new(@"C:\WebDrivers\FirefoxProfile-DetaultUser");
        FirefoxOptions firefoxOptions = new();
        firefoxOptions.Profile = firefoxProfile;
        firefoxOptions.AddArguments("--headless");
        FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, firefoxOptions, TimeSpan.FromSeconds(30));
    }
    public void AllocateWebDrivers(
        List<AddressModel> addressList, int amountToScrape)
    {
        WebDriverModel webDriverModel = new()
        {
            Driver = FirefoxDriver,
            Input = FirefoxInput,
            AddressList = addressList
        };

        List<Task> tasks = new();
        tasks.Add(Task.Run(() => Scrape(webDriverModel)));
        Task.WaitAll(tasks.ToArray());

    }
    public async Task Scrape(WebDriverModel webDriverModel)
    {
        totalCount = webDriverModel.AddressList.Count;
        bool result;
        bool checkingIfAddressExists = true;

        WebDriverWait webDriverWait = new(webDriverModel.Driver, TimeSpan.FromSeconds(10));
        webDriverWait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotSelectableException),
            typeof(ElementNotVisibleException));

        try
        {
            Console.WriteLine($"{webDriverModel.Driver} begin...");
            var iterList = webDriverModel.AddressList.ToList();
            foreach (var address in iterList)
            {
                currentCount = iterList.IndexOf(address) + 1;
                // Selecting "MONTGOMERY COUNTY"
                webDriverModel.Driver.Navigate().GoToUrl(BaseUrl);
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(17)")));
                webDriverModel.Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlSearchType > option:nth-child(3)")));
                webDriverModel.Input.Click();

                // Click Continue button
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue")));
                webDriverModel.Input.Click();

                // Input Ward
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtDistrict")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Ward);

                // Input AccountNumber
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtAccountIdentifier")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.AccountNumber);

                // Click Next button
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                webDriverModel.Input.Click();
                if (webDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
                {
                    if (webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                        .Text.Contains("There are no records that match your criteria"))
                    {
                        // Address does not exist in SDAT
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var montgomeryCountyDataService = _montgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await montgomeryCountyDataService.Delete(address.AccountId);
                            Console.WriteLine($"{address.AccountId.Trim()} does not exist and was deleted.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{webDriverModel.Driver} found {address.AccountId.Trim()} does not exist and tried to delete, but the error message text is different than usual: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        webDriverModel.Driver.Quit();
                    }
                }
                else
                {
                    // Click Ground Rent Registration link
                    webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                    webDriverModel.Input.Click();

                    // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                    if (webDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                    {
                        if (webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                            .Text.Contains("There is currently no ground rent"))
                        {
                            // Property is not ground rent
                            address.IsGroundRent = false;
                            using (var uow = _dataContext.CreateUnitOfWork())
                            {
                                var montgomeryCountyDataService = _montgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                                result = await montgomeryCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                                {
                                    AccountId = address.AccountId,
                                    IsGroundRent = address.IsGroundRent
                                });
                                webDriverModel.AddressList.Remove(address);
                            }
                            if (result is false)
                            {
                                // Something wrong happened and I do not want the application to skip over this address
                                webDriverModel.Driver.Quit();
                                Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Quitting scrape.");
                            }
                        }
                        else
                        {
                            webDriverModel.Driver.Quit();
                            Console.WriteLine($"{webDriverModel.Driver} found {address.AccountId.Trim()} has a different error message than 'There is currently no ground rent' which is: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        }
                    }
                    else
                    {
                        // Property must be ground rent
                        address.IsGroundRent = true;
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var montgomeryCountyDataService = _montgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
                            result = await montgomeryCountyDataService.CreateOrUpdateSDATScraper(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent
                            });
                            webDriverModel.AddressList.Remove(address);
                        }
                        if (result is false)
                        {
                            // Something wrong happened and I do not want the application to skip over this address
                            webDriverModel.Driver.Quit();
                            Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Quitting scrape.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            await RestartScrape(webDriverModel);
        }
        finally
        {
            await RestartScrape(webDriverModel);
        }
    }
    private async Task RestartScrape(WebDriverModel webDriverModel)
    {
        webDriverModel.AddressList.Clear();
        using (var uow = _dataContext.CreateUnitOfWork())
        {
            var montgomeryCountyDataService = _montgomeryCountyDataServiceFactory.CreateGroundRentProcessorDataService(uow);
            AddressList = await montgomeryCountyDataService.ReadTopAmountWhereIsGroundRentNull(amountToScrape);
        }
        if (AddressList.Count == 0)
        {
            webDriverModel.Driver.Quit();
            ReportTotals(webDriverModel);
        }
        else
        {
            Console.WriteLine("Restarting scrape.");
            AllocateWebDrivers(AddressList, amountToScrape);
        }
    }
    private void ReportTotals(WebDriverModel webDriverModel)
    {
        percentComplete = decimal.Divide(currentCount, totalCount);
        Console.WriteLine($"{webDriverModel.Driver} has processed {percentComplete:P0} of addresses in list.");
    }
}
