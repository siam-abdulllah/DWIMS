using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentAprComment : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string MarketGroupCode { get; set; }
        public string MarketGroupName { get; set; }
        public string Comments { get; set; }
        public string AprStatus { get; set; }
    }
}
