using DataLibrary.Models;

namespace DataLibrary.Services.SDATScrapers;

public interface IRealPropertySearchScraper
{
    void AllocateWebDrivers(List<AddressModel> firefoxAddressList, int amountToScrape);
    Task Scrape(WebDriverModel webDriverModel);
}