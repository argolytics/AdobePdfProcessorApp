using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;
public class AddressDataServiceFactory : IGroundRentProcessorDataServiceFactory
{
    public IGroundRentProcessorDataService CreateGroundRentProcessorDataService(IUnitOfWork uow) => new AddressSqlDataService(uow);
}
