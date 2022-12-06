using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IAddressDataService
    {
        Task CreateOrUpdateFromSpecPrintFileForBaltimoreCity(AddressModel addressModel);
        Task CreateOrUpdateFromSpecPrintFileForBaltimoreCounty(AddressModel addressModel);
        Task CreateOrUpdateFromMDOpenDatasetCsvFile(AddressModel addressModel);
        Task CreateOrUpdateFromSDATRedeemedFile(AddressModel addressModel);
        Task<bool> CreateOrUpdateIsGroundRent(AddressModel addressModel);
        Task<bool> CreateOrUpdateIsGroundRentBaltimoreCity1(AddressModel addressModel);
        Task<bool> CreateOrUpdateIsGroundRentBaltimoreCity2(AddressModel addressModel);
        Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNull(int amount);
        Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNullAndYearBuiltZeroBaltimoreCity1(int amount);
        Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNullAndYearBuiltZeroBaltimoreCity2(int amount);
        Task<bool> DeleteAddress(string id);
        Task<bool> DeleteBaltimoreCity1(string accountId);
        Task<bool> DeleteBaltimoreCity2(string accountId);
    }
}