using Core.Entities;
using System;

namespace API.Dtos
{
    public class ApprovalCeilingDto
    {
        public int Id { get; set; }
        public int ApprovalAuthorityId { get; set; }
        public ApprovalAuthorityToReturnDto ApprovalAuthority { get; set; }
        public int DonationId { get; set; }
        public Donation Donation { get; set; }
        public DateTimeOffset? InvestmentFrom { get; set; }
        public DateTimeOffset? InvestmentTo { get; set; }
        public int AmountPerTransacion { get; set; }
        public int AmountPerMonth { get; set; }
        public string Additional { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public DateTimeOffset SetOn { get; set; }
    }
}
