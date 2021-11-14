using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ApprovalCeiling : BaseEntity
    {
        public int ApprovalAuthorityId { get; set; }
        [ForeignKey("ApprovalAuthorityId")]
        public ApprovalAuthority ApprovalAuthority { get; set; }
        public int DonationId { get; set; }
        [ForeignKey("DonationId")]
        public Donation Donation { get; set; }
        public DateTimeOffset? InvestmentFrom { get; set; }
        public DateTimeOffset? InvestmentTo { get; set; }
        public int AmountPerTransacion { get; set; }
        public int AmountPerMonth { get; set; }
        public string Additional { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
    }
}
