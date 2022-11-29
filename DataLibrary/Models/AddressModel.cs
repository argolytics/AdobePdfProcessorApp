using DataLibrary.Helpers;

namespace DataLibrary.Models
{
    public class AddressModel
    {
        public string? AccountId { get; set; }
        public string? Ward { get; set; }
        public string? Section { get; set; }
        public string? Block { get; set; }
        public string? Lot { get; set; }
        public string? LandUseCode { get; set; }
        public int? YearBuilt { get; set; }
        public bool? IsRedeemed { get; set; }
        public bool? IsGroundRent { get; set; }
        public string? lastAmendmentDetails { get; set; }
        public bool? DetailsNotLegible { get; set; }
        public double? PaymentAmount { get; set; }
        public string? PaymentFrequency { get; set; }
        public DateTime? PaymentDateAnnual { get; set; }
        public DateTime? PaymentDateSemiAnnual1 { get; set; }
        public DateTime? PaymentDateSemiAnnual2 { get; set; }
        public DateTime? PaymentDateQuarterly1 { get; set; }
        public DateTime? PaymentDateQuarterly2 { get; set; }
        public DateTime? PaymentDateQuarterly3 { get; set; }
        public DateTime? PaymentDateQuarterly4 { get; set; }
    }
}
