using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvestmentTargetGroupStatusDto
    {
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int? MarketGroupMstId { get; set; }
        public MarketGroupMst MarketGroupMst { get; set; }
        public bool? CompletionStatus { get; set; }
    }
}
