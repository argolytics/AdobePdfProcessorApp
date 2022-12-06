namespace DataLibrary.Models
{
    public class AddressModel
    {
        public string? AccountId { get; set; }
        public string? AccountNumber { get; set; }
        public string? Ward { get; set; }
        public string? Section { get; set; }
        public string? Block { get; set; }
        public string? Lot { get; set; }
        public string? LandUseCode { get; set; }
        public int? YearBuilt { get; set; }
        public bool? IsRedeemed { get; set; }
        public bool? IsGroundRent { get; set; }
        public bool? IsLegible { get; set; }
        public enum NotLegible { PaymentAmount, PaymentDate }
        public List<NotLegible> NotLegibleList { get; set; } = new List<NotLegible>();
        public double? PaymentAmount { get; set; }
        public enum PaymentFrequency { Annual, SemiAnnual, Quarterly, Other }
        public PaymentFrequency GroundRentPaymentFrequency { get; set; }
        public DateTime? PaymentDateAnnual { get; set; }
        public DateTime? PaymentDateSemiAnnual1 { get; set; }
        public DateTime? PaymentDateSemiAnnual2 { get; set; }
        public DateTime? PaymentDateQuarterly1 { get; set; }
        public DateTime? PaymentDateQuarterly2 { get; set; }
        public DateTime? PaymentDateQuarterly3 { get; set; }
        public DateTime? PaymentDateQuarterly4 { get; set; }
        public DateTime? PaymentDateOther { get; set; }
    }
}
