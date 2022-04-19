using Core.Entities;

namespace API.Dtos
{
    public class InvestmentAprCommentDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string PostingType { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string SBU { get; set; }
        public string Comments { get; set; }
        public string AprStatus { get; set; }
    }
}
