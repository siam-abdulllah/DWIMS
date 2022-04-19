using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class InvestmentTargetGroupSQL : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string MarketGroupName { get; set; }
        public int Priority { get; set; }
        public string RecStatus { get; set; }
        public string ApprovalAuthorityName { get; set; }
    }
}
