using DataLibrary.Helpers;

namespace DataLibrary.Models
{
    public class AddressModel
    {
        public string? AccountId { get => StringTrimmer.Trimmer(accountId); }
        public bool? IsRedeemed { get => ElementFinder.DeterminePropertyRedemptionStatus(lastAmendmentDetails, "PAY"); }
        public string? accountId { get; set; }
        public string? lastAmendmentDetails { get; set; }
    }
}
