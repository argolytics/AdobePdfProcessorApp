using DataLibrary.Models;
using Dapper;
using System.Data;
using DataLibrary.DbAccess;

namespace DataLibrary.DbServices;

public class AddressSqlDataService : IAddressDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressSqlDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateOrUpdateAddress(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.IsRedeemed
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdate", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task<AddressModel> ReadAddressById(int accountId)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spAddress_ReadById", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
    public async Task UpdateAddress(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.IsRedeemed
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_Update", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task DeleteAddress(int accountId)
    {
        await _unitOfWork.Connection.ExecuteAsync("spAddress_Delete", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
}
