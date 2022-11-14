using CsvHelper.Configuration;

namespace DataLibrary.Models;
public class AddressClassMap : ClassMap<AddressModel>
{
    public AddressClassMap()
    {
        Map(m => m.accountId).Name(@"ACCOUNT NO.  -----------  ");
        Map(m => m.lastAmendmentDetails).Name(@"--------------------------------------------------------------------------------  LAST AMENDMENT DETAILS  ----------------------  ");
    }
}