using DataLibrary.Helpers;

namespace DataLibrary.Models
{
    public class AddressModel
    {
        public string? AccountId { get => StringTrimmer.Trimmer(accountId); }
        public int? CapitalizedGroundRent1Amount { get; set; }
        public int? CapitalizedGroundRent2Amount { get; set; }
        public int? CapitalizedGroundRent3Amount { get; set; }
        public bool? IsRedeemed { get => ElementFinder.DeterminePropertyRedemptionStatus(lastAmendmentDetails, "PAY"); }
        public string? accountId { get; set; }
        public string? lastAmendmentDetails { get; set; }
    }
}
