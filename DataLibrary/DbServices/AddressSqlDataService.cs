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
    public async Task CreateOrUpdateFromSpecPrintFileForBaltimoreCity(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.Ward,
            addressModel.Section,
            addressModel.Block,
            addressModel.Lot,
            addressModel.LandUseCode,
            addressModel.YearBuilt
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateSpecPrintFileForBaltimoreCity", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task CreateOrUpdateFromSpecPrintFileForBaltimoreCounty(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.AccountNumber,
            addressModel.Ward,
            addressModel.LandUseCode,
            addressModel.YearBuilt
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateSpecPrintFileForBaltimoreCounty", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task CreateOrUpdateFromMDOpenDatasetCsvFile(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.AccountNumber,
            addressModel.Ward,
            addressModel.Section,
            addressModel.Block,
            addressModel.Lot,
            addressModel.LandUseCode,
            addressModel.YearBuilt
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateMDOpenDatasetCsvFile", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task CreateOrUpdateFromSDATRedeemedFile(AddressModel addressModel)
    {
        var parms = new
        {
            addressModel.AccountId,
            addressModel.IsRedeemed
        };
        await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateSDATRedeemedFile", parms,
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
    }
    public async Task<bool> CreateOrUpdateIsGroundRent(AddressModel addressModel)
    {
        try
        {
            var parms = new
            {
                addressModel.AccountId,
                addressModel.IsGroundRent
            };
            await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateIsGroundRent", parms,
                commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<bool> CreateOrUpdateIsGroundRentBaltimoreCity1(AddressModel addressModel)
    {
        try
        {
            var parms = new
            {
                addressModel.AccountId,
                addressModel.IsGroundRent
            };
            await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateIsGroundRentBaltimoreCity1", parms,
                commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<bool> CreateOrUpdateIsGroundRentBaltimoreCity2(AddressModel addressModel)
    {
        try
        {
            var parms = new
            {
                addressModel.AccountId,
                addressModel.IsGroundRent
            };
            await _unitOfWork.Connection.ExecuteAsync("spAddress_CreateOrUpdateIsGroundRentBaltimoreCity2", parms,
                commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNull(int amount)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spAddress_ReadTopAmountWhereIsGroundRentNullAndYearBuiltIsZero", new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNullAndYearBuiltZeroBaltimoreCity1(int amount)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spAddress_ReadTopAmountWhereIsGroundRentNullAndYearBuiltIsZeroBaltimoreCity1", new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<List<AddressModel>> ReadAddressTopAmountWhereIsGroundRentNullAndYearBuiltZeroBaltimoreCity2(int amount)
    {
        return (await _unitOfWork.Connection.QueryAsync<AddressModel>("spAddress_ReadTopAmountWhereIsGroundRentNullAndYearBuiltIsZeroBaltimoreCity2", new { Amount = amount },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction)).ToList();
    }
    public async Task<bool> DeleteAddress(string accountId)
    {
        try
        {
            await _unitOfWork.Connection.ExecuteAsync("spAddress_Delete", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<bool> DeleteBaltimoreCity1(string accountId)
    {
        try
        {
            await _unitOfWork.Connection.ExecuteAsync("spBaltimoreCity1_Delete", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
    public async Task<bool> DeleteBaltimoreCity2(string accountId)
    {
        try
        {
            await _unitOfWork.Connection.ExecuteAsync("spBaltimoreCity2_Delete", new { AccountId = accountId },
            commandType: CommandType.StoredProcedure, transaction: _unitOfWork.Transaction);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
