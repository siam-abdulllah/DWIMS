using System;

namespace Core.Entities
{
    public class ApprovalCeiling : BaseEntity
    {
        public int ApprovalAuthorityId { get; set; }
        public ApprovalAuthority ApprovalAuthority { get; set; }
        public string DonationType { get; set; }
        public DateTimeOffset? InvestmentFrom { get; set; }
        public DateTimeOffset? InvestmentTo { get; set; }
        public int AmountPerTransacion { get; set; }
        public int AmountPerMonth { get; set; }
        public string Additional { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
    }
}
