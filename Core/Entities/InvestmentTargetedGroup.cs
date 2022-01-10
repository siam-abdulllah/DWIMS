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
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string MarketGroupCode { get; set; }
        public string MarketGroupName { get; set; }
        public int? MarketGroupMstId { get; set; }
        [ForeignKey("MarketGroupMstId")]
        public MarketGroupMst MarketGroupMst { get; set; }
        public bool? CompletionStatus { get; set; }
    }
}
