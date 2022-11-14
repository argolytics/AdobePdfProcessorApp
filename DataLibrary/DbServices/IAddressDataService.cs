using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IAddressDataService
    {
        Task CreateOrUpdateAddress(AddressModel addressModel);
        Task<AddressModel> ReadAddressById(int id);
        Task UpdateAddress(AddressModel addressModel);
        Task DeleteAddress(int id);
    }
}