using DataLibrary.DbServices;
using DataLibrary.Models;

namespace DataLibrary.Services.SDATScrapers;

public interface IRealPropertySearchScraper
{
    void AllocateWebDrivers(
        List<AddressModel> firefoxAddressList);
    Task Scrape(WebDriverModel webDriverModel);
}