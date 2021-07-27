using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    class InvestmentTargetedGroup
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int MarketGroupId { get; set; }
        [ForeignKey("MarketGroupId")]
        public MarketGroupMst MarketGroupMst { get; set; }
    }
}
