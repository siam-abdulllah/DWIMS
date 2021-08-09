using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentTargetedGroup : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int? MarketGroupMstId { get; set; }
        [ForeignKey("MarketGroupMstId")]
        public MarketGroupDtl MarketGroupMst { get; set; }
    }
}
