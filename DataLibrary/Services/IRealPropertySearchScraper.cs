using DataLibrary.Models;

namespace DataLibrary.Services;

public interface IRealPropertySearchScraper
{
    Task AllocateWebDrivers(List<AddressModel> chromeAddressList, List<AddressModel> edgeAddressList);
    Task Scrape(WebDriverModel webDriverModel);
}