using System;

namespace API.Dtos
{
    public class ApprovalCeilingDto
    {
        public int Id { get; set; }
        public ApprovalAuthorityToReturnDto ApprovalAuthority { get; set; }
        public InvestmentTypeDto InvestmentType { get; set; }
        public DateTime? InvestmentFrom { get; set; }
        public DateTime? InvestmentTo { get; set; }
        public int TransacionAmount { get; set; }
        public string Additional { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }
        public DateTimeOffset SetOn { get; set; }
    }
}
