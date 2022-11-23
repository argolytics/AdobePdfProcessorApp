using DataLibrary.Models;

namespace DataLibrary.DbServices
{
    public interface IAddressDataService
    {
        Task CreateOrUpdateSDAT(AddressModel addressModel);
        Task CreateOrUpdateSpecPrint(AddressModel addressModel);
        Task<AddressModel> ReadAddressById(int id);
        Task DeleteAddress(int id);
    }
}