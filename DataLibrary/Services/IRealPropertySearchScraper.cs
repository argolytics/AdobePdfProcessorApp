using DataLibrary.DbServices;
using DataLibrary.Models;

namespace DataLibrary.Services;

public interface IRealPropertySearchScraper
{
    void AllocateWebDrivers(
        List<AddressModel> firefoxAddressList);
    Task Scrape(WebDriverModel webDriverModel);
}