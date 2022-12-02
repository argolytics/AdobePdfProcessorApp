using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services;

public class RealPropertySearchScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly IAddressDataServiceFactory _addressDataServiceFactory;
    private readonly HubConnectionContext _context;
    private WebDriver ChromeDriver { get; set; } = null;
    private WebDriver EdgeDriver { get; set; } = null;
    private WebDriver FirefoxDriver { get; set; } = null;
    private WebDriver IEDriver { get; set; } = null;
    private IWebElement ChromeInput { get; set; }
    private IWebElement EdgeInput { get; set; }
    private IWebElement FirefoxInput { get; set; }
    private IWebElement IEInput { get; set; }
    private string ChromeDriverPath { get; set; } = @"C:\WebDrivers\chromedriver.exe";
    private string EdgeDriverPath { get; set; } = @"C:\WebDrivers\msedgedriver.exe";
    private string FirefoxDriverPath { get; set; } = @"C:\WebDrivers\geckodriver.exe";
    private string IEDriverPath { get; set; } = @"C:\WebDrivers\IEWebDriver";
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private bool IsConnected { get; set; }

    public RealPropertySearchScraper(
        IDataContext dataContext,
        IAddressDataServiceFactory addressDataServiceFactory)
    {
        _dataContext = dataContext;
        _addressDataServiceFactory = addressDataServiceFactory;

        //var chromeOptions = new ChromeOptions();
        //chromeOptions.AddArguments("--headless");
        //ChromeDriver = new ChromeDriver(ChromeDriverPath, chromeOptions, TimeSpan.FromSeconds(30));
        
        //var edgeOptions = new EdgeOptions();
        //edgeOptions.AddArguments("--headless");
        //EdgeDriver = new EdgeDriver(EdgeDriverPath, edgeOptions, TimeSpan.FromSeconds(30));

        FirefoxProfile firefoxProfile = new(@"C:\WebDrivers\FirefoxProfile-DetaultUser");
        FirefoxOptions firefoxOptions = new();
        firefoxOptions.Profile = firefoxProfile;
        firefoxOptions.AddArguments("--headless");
        FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, firefoxOptions, TimeSpan.FromSeconds(30));

        //var ieOptions = new InternetExplorerOptions();
        //ieOptions.IgnoreZoomLevel = true;
        //IEDriver = new InternetExplorerDriver(IEDriverPath, ieOptions, TimeSpan.FromSeconds(30));
    }
    public void AllocateWebDrivers(
        List<AddressModel> firefoxAddressList)
    {
        //WebDriverModel chromeDriverModel = new WebDriverModel 
        //{ 
        //    Driver = ChromeDriver,
        //    Input = ChromeInput,
        //    AddressList = chromeAddressList
        //};
        //WebDriverModel edgeDriverModel = new WebDriverModel
        //{
        //    Driver = EdgeDriver,
        //    Input = EdgeInput,
        //    AddressList = edgeAddressList
        //};
        WebDriverModel firefoxDriverModel = new WebDriverModel
        {
            Driver = FirefoxDriver,
            Input = FirefoxInput,
            AddressList = firefoxAddressList
        };
        //WebDriverModel ieDriverModel = new WebDriverModel
        //{
        //    Driver = IEDriver,
        //    Input = IEInput,
        //    AddressList = ieAddressList
        //};

        List<Task> tasks = new();
        //tasks.Add(Task.Run(() => Scrape(chromeDriverModel)));
        //tasks.Add(Task.Run(() => Scrape(edgeDriverModel)));
        tasks.Add(Task.Run(() => Scrape(firefoxDriverModel)));
        //tasks.Add(Task.Run(() => Scrape(ieDriverModel)));
        Task.WaitAll(tasks.ToArray());

    }
    public async Task Scrape(WebDriverModel webDriverModel)
    {
        int currentCount;
        var totalCount = webDriverModel.AddressList.Count;
        WebDriverWait webDriverWait = new(webDriverModel.Driver, TimeSpan.FromSeconds(30));
        webDriverWait.IgnoreExceptionTypes(
            typeof(NoSuchElementException),
            typeof(StaleElementReferenceException),
            typeof(ElementNotSelectableException),
            typeof(ElementNotVisibleException));

        try
        {
            Console.WriteLine($"{webDriverModel.Driver} begin...");
            foreach (var address in webDriverModel.AddressList)
            {
                currentCount = webDriverModel.AddressList.IndexOf(address) + 1;
                // Selecting "BALTIMORE CITY"
                webDriverModel.Driver.Navigate().GoToUrl(BaseUrl);
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlCounty > option:nth-child(4)")));
                webDriverModel.Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucSearchType_ddlSearchType > option:nth-child(3)")));
                webDriverModel.Input.Click();

                // Click Continue button
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue")));
                webDriverModel.Input.Click();

                // ChromeInput Ward
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtWard")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Ward);

                // ChromeInput Section
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtSection")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Section);

                // ChromeInput Block
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtBlock")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Block);

                // ChromeInput Lot
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtLot")));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Lot);

                // Click Next button
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
                webDriverModel.Input.Click();

                // Click Ground Rent Registration link
                webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0")));
                webDriverModel.Input.Click();

                // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                bool result;
                if (webDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Count != 0)
                {
                    if (webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr"))
                        .Text.Contains("There is currently no ground rent"))
                    {
                        // Property is not ground rent
                        address.IsGroundRent = false;
                        using (var uow = _dataContext.CreateUnitOfWork())
                        {
                            var addressDataService = _addressDataServiceFactory.CreateAddressDataService(uow);
                            result = await addressDataService.CreateOrUpdateGroundRentJasonFromSDATIsGroundRent(new AddressModel
                            {
                                AccountId = address.AccountId,
                                IsGroundRent = address.IsGroundRent
                            });
                            //var addressDataService = _addressDataServiceFactory.CreateAddressDataService(uow);
                            //result = await addressDataService.CreateOrUpdateGroundRentAmandaFromSDATIsGroundRent(new AddressModel
                            //{
                            //    AccountId = address.AccountId,
                            //    IsGroundRent = address.IsGroundRent
                            //});
                            Console.WriteLine($"{address.AccountId.Trim()} is fee simple.");
                        }
                        if (result is false)
                        {
                            // Something wrong happened and I do not want the application to skip over this address
                            webDriverModel.Driver.Quit();
                            Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Call Jason.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"{webDriverModel.Driver} found {address.AccountId.Trim()} has a different error message than 'There is currently no ground rent' which is: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        webDriverModel.Driver.Quit();
                    }
                }
                else
                {
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
                        Console.WriteLine($"{address.AccountId.Trim()} is ground rent.");
                    }
                    if (result is false)
                    {
                        // Something wrong happened and I do not want the application to skip over this address
                        webDriverModel.Driver.Quit();
                        Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Call Jason.");
                    }
                }
                decimal percentComplete = Decimal.Divide(currentCount, totalCount);
                Console.WriteLine($"{webDriverModel.Driver} has processed {percentComplete:P0} of addresses in list.");
            }
        }
        catch (NoSuchElementException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        catch (StaleElementReferenceException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        catch (ArgumentNullException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        catch (WebDriverException ex)
        {
            Console.WriteLine($"{webDriverModel.Driver} ran into the following exception: {ex.Message}");
            Thread.Sleep(3000);
        }
        webDriverModel.Driver.Quit();
    }
}
