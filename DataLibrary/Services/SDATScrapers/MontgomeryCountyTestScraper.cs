using DataLibrary.DbAccess;
using DataLibrary.DbServices;
using DataLibrary.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace DataLibrary.Services.SDATScrapers;

public class MontgomeryCountyTestScraper : IRealPropertySearchScraper
{
    private WebDriver FirefoxDriver { get; set; } = null;
    private IWebElement FirefoxInput { get; set; }
    private string FirefoxDriverPath { get; set; } = @"C:\WebDrivers\geckodriver.exe";
    private string BaseUrl { get; set; } = "https://sdat.dat.maryland.gov/RealProperty/Pages/default.aspx";

    public MontgomeryCountyTestScraper()
    {
        FirefoxProfile firefoxProfile = new(@"C:\WebDrivers\FirefoxProfile-DetaultUser");
        FirefoxOptions firefoxOptions = new();
        firefoxOptions.Profile = firefoxProfile;
        firefoxOptions.AddArguments("--headless");
        FirefoxDriver = new FirefoxDriver(FirefoxDriverPath, firefoxOptions, TimeSpan.FromSeconds(30));
    }
    public void AllocateWebDrivers(
        List<AddressModel> firefoxAddressList)
    {
        WebDriverModel firefoxDriverModel = new WebDriverModel
        {
            Driver = FirefoxDriver,
            Input = FirefoxInput,
            AddressList = firefoxAddressList
        };

        List<Task> tasks = new();
        tasks.Add(Task.Run(() => Scrape(firefoxDriverModel)));
        Task.WaitAll(tasks.ToArray());

    }
    public async Task Scrape(WebDriverModel webDriverModel)
    {
        int currentCount;
        //var totalCount = webDriverModel.AddressList.Count;
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
            // Selecting "BALTIMORE COUNTY"
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
            webDriverModel.Input.SendKeys("01");

            // Input AccountNumber
            webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementExists(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucEnterData_txtAccountIdentifier")));
            webDriverModel.Input.Clear();
            webDriverModel.Input.SendKeys("00012691");

            // Click Next button
            webDriverModel.Input = webDriverWait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_StepNavigationTemplateContainerID_btnStepNextButton")));
            webDriverModel.Input.Click();

            if (webDriverModel.Driver.FindElements(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr")).Count != 0)
            {
                if (webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_lblErr"))
                    .Text.Contains("There are no records that match your criteria"))
                {
                    // Address does not exist in SDAT
                    Console.WriteLine($" does not exist and was deleted.");
                }
                else
                {
                    Console.WriteLine($"{webDriverModel.Driver} found does not exist and tried to delete, but the error message text is different than usual: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
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
                        Console.WriteLine($" is fee simple.");
                    }
                    else
                    {
                        Console.WriteLine($"{webDriverModel.Driver} found has a different error message than 'There is currently no ground rent' which is: {webDriverModel.Driver.FindElement(By.CssSelector("#cphMainContentArea_ucSearchType_wzrdRealPropertySearch_ucGroundRent_lblErr")).Text}. Quitting scrape.");
                        webDriverModel.Driver.Quit();
                    }
                }
                else
                {
                    // Property must be ground rent
                    Console.WriteLine($" is ground rent.");
                }
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
        Console.WriteLine("Scrape complete.");
    }
}
