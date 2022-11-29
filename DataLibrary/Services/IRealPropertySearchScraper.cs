using DataLibrary.Models;

namespace DataLibrary.Services
{
    public interface IRealPropertySearchScraper
    {
        Task Scrape(List<AddressModel> addressList);
    }
}