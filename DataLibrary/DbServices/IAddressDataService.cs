using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IAddressDataService
    {
        Task CreateOrUpdateFromSpecPrintFile(AddressModel addressModel);
        Task CreateOrUpdateFromSDATRedeemedFile(AddressModel addressModel);
        Task<bool> CreateOrUpdateFromSDATIsGroundRent(AddressModel addressModel);
        Task<AddressModel> ReadAddressByAccountId(int accountId);
        Task<List<AddressModel>> ReadGroundRentJasonTopAmountWhereIsGroundRentNull();
        Task<List<AddressModel>> ReadGroundRentAmandaTopAmountWhereIsGroundRentNull();
        Task DeleteAddress(int id);
    }
}