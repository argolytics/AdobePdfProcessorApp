using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using Microsoft.AspNetCore.SignalR;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace DataLibrary.Services;

public class RealPropertySearchScraper : IRealPropertySearchScraper
{
    private readonly IDataContext _dataContext;
    private readonly IAddressDataServiceFactory _addressDataServiceFactory;
    private readonly HubConnectionContext _context;
    private WebDriver ChromeDriver { get; set; } = null;
    private WebDriver EdgeDriver { get; set; } = null;
    private IWebElement ChromeInput { get; set; }
    private IWebElement EdgeInput { get; set; }
    private List<WebDriverModel> WebDriverList { get; set; } = new();
    private string ChromeDriverPath { get; set; } = @"C:\ChromeDriver\chromedriver.exe";
    private string EdgeDriverPath { get; set; } = @"C:\EdgeDriver\msedgedriver.exe";
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";
    private bool IsConnected { get; set; }

    public RealPropertySearchScraper(
        IDataContext dataContext,
        IAddressDataServiceFactory addressDataServiceFactory)
    {
        _dataContext = dataContext;
        _addressDataServiceFactory = addressDataServiceFactory;

        var chromeOptions = new ChromeOptions();
        chromeOptions.AddArguments("--headless");
        ChromeDriver = new ChromeDriver(ChromeDriverPath, chromeOptions, TimeSpan.FromSeconds(30));
        ChromeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

        var edgeOptions = new EdgeOptions();
        edgeOptions.AddArguments("--headless");
        EdgeDriver = new EdgeDriver(EdgeDriverPath, edgeOptions, TimeSpan.FromSeconds(30));
        EdgeDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
    }
    public Task AllocateWebDrivers(List<AddressModel> chromeAddressList, List<AddressModel> edgeAddressList)
    {
        WebDriverModel chromeDriverModel = new WebDriverModel 
        { 
            Driver = ChromeDriver,
            Input = ChromeInput,
            AddressList = chromeAddressList
        };
        WebDriverModel edgeDriverModel = new WebDriverModel
        {
            Driver = EdgeDriver,
            Input = EdgeInput,
            AddressList = edgeAddressList
        };
        WebDriverList.Add(chromeDriverModel);
        WebDriverList.Add(edgeDriverModel);

        return Task.WhenAll(WebDriverList.Select(webDriverModel => Scrape(webDriverModel)));
    }
    public async Task Scrape(WebDriverModel webDriverModel)
    {
        int currentCount;
        var totalCount = webDriverModel.AddressList.Count;

        try
        {
            Console.WriteLine($"{webDriverModel.Driver} begin...");
            foreach (var address in webDriverModel.AddressList)
            {
                currentCount = webDriverModel.AddressList.IndexOf(address) + 1;
                Thread.Sleep(2000);
                // Selecting "BALTIMORE CITY"
                webDriverModel.Driver.Navigate().GoToUrl(BaseUrl);
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("/html/body/form/div[4]/div[4]/div/div/main/div[3]/div/div/table/tbody/tr[1]/td/div/div/fieldset/div[1]/div[2]/select/option[4]"));
                webDriverModel.Input.Click();

                // Selecting "PROPERTY ACCOUNT IDENTIFIER"
                Thread.Sleep(200);
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("/html/body/form/div[4]/div[4]/div/div/main/div[3]/div/div/table/tbody/tr[1]/td/div/div/fieldset/div[2]/div[2]/select/option[3]"));
                webDriverModel.Input.Click();

                // Click Continue button
                Thread.Sleep(200);
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StartNavigationTemplateContainerID_btnContinue\"]"));
                webDriverModel.Input.Click();
                Thread.Sleep(3000);

                // ChromeInput Ward
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtWard\"]"));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Ward);

                // ChromeInput Section
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtSection\"]"));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Section);

                // ChromeInput Block
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtBlock\"]"));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Block);

                // ChromeInput Lot
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtLot\"]"));
                webDriverModel.Input.Clear();
                webDriverModel.Input.SendKeys(address.Lot);

                // Click Next button
                Thread.Sleep(200);
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton\"]"));
                webDriverModel.Input.Click();
                Thread.Sleep(3000);

                // Click Ground Rent Registration link
                webDriverModel.Input = webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucDetailsSearch_dlstDetaisSearch_lnkGroundRentRegistration_0\"]"));
                webDriverModel.Input.Click();
                Thread.Sleep(1000);

                // Condition: check if html has ground rent error tag (meaning property has no ground rent registered)
                bool result;
                if (webDriverModel.Driver.FindElements(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr\"]")).Count != 0)
                {
                    if (webDriverModel.Driver.FindElement(By.XPath("//*[@id=\"cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr\"]"))
                        .Text.Contains("There is currently no ground rent"))
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
                            Console.WriteLine($"{address.AccountId.Trim()} is fee simple.");
                        }
                        if (result is false)
                        {
                            // Something wrong happened and I do not want the application to skip over this address
                            webDriverModel.Driver.Quit();
                            Console.WriteLine($"Db could not complete transaction for {address.AccountId.Trim()}. Call Jason.");
                        }
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
        catch (NoSuchElementException)
        {

            //IsConnected = false;
            //if (AttemptReconnect())
            //{
            //    IsConnected = true;
            //}
        }
        catch (StaleElementReferenceException)
        {

            //IsConnected = false;
            //if (AttemptReconnect())
            //{
            //    IsConnected = true;
            //}
        }
        catch (ArgumentNullException)
        {

            //IsConnected = false;
            //if (AttemptReconnect())
            //{
            //    IsConnected = true;
            //}
        }
        catch (WebDriverException)
        {

            //IsConnected = false;
            //if (AttemptReconnect())
            //{
            //    IsConnected = true;
            //}
        }
        //if (IsConnected)
        //{
        //    continue; // If the connection is re-established, continue the foreach loop
        //}
        finally
        {
            webDriverModel.Driver.Quit();
        }
    }
    //private bool AttemptReconnect()
    //{
    //    for (int i = 0; i < 3; i++) // Make 3 attempts to connect via Selenium scraper
    //    {
    //        Console.WriteLine($"Internet connectivity problem. Reconnecting attempt {i} of 3.");
    //        Thread.Sleep(5000);
    //        webDriverModel.Input.Click();
    //        Thread.Sleep(2000);
    //        if (_context.GetHttpContext().Connection != null)
    //        {
    //            return true;
    //        }
    //    }
    //    return false;
    //}
}
