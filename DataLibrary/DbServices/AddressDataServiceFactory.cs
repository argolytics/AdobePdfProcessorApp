using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;
public class AddressDataServiceFactory : IAddressDataServiceFactory
{
    public IAddressDataService CreateAddressDataService(IUnitOfWork uow) => new AddressSqlDataService(uow);
}
