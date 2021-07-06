using System;

namespace Core.Entities
{
    public class ApprovalCeiling : BaseEntity
    {
        public int ApprovalAuthorityId { get; set; }
        public ApprovalAuthority ApprovalAuthority { get; set; }
        public int InvestmentTypeId { get; set; }
        public InvestmentType InvestmentType { get; set; }
        public DateTime? InvestmentFrom { get; set; }
        public DateTime? InvestmentTo { get; set; }
        public int TransacionAmount { get; set; }
        public string Additional { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
    }
}
