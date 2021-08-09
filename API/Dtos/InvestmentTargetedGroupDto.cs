using System;

namespace API.Dtos
{
    public class InvestmentTargetedGroupDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public int? MarketGroupMstId { get; set; }
        public MarketGroupMstDto MarketGroupMst { get; set; }
    }
}