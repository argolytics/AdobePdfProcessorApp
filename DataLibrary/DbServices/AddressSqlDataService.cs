using System.Data;
using DataLibrary.Models;
using DataLibrary.DbAccess;
using Dapper;

namespace DataLibrary.DbServices;

public class AddressSqlDataService : IAddressDataService
{
    private readonly IUnitOfWork _unitOfWork;

    public AddressSqlDataService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public async Task CreateOrUpdateSDAT(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.IsRedeemed
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateSDAT", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task CreateOrUpdateSpecPrint(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.CapitalizedGroundRent1Amount,
            addressModel.CapitalizedGroundRent2Amount,
            addressModel.CapitalizedGroundRent3Amount
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateSpecPrint", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task<AddressModel> ReadAddressById(int accountId)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spAddress_ReadById", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).FirstOrDefault();
    }
    public async Task DeleteAddress(int accountId)
    {
        await _unitOfWork.Connection.ExecuteAsync("spAddress_Delete", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
}
