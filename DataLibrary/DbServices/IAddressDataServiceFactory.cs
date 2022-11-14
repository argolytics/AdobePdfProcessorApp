using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;
public interface IAddressDataServiceFactory
{
    IAddressDataService CreateAddressDataService(IUnitOfWork uow);
}