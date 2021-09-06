using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RptInsSocBcdsInvestmentDto
    {
        public int Id { get; set; }
        public string SBUName { get; set; }
        public string SBUCode { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public string SocietyId { get; set; }
        public string SocietyName { get; set; }
        public string BcdsId { get; set; }
        public string BcdsName { get; set; }
        public string DonationType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double InvestedAmt { get; set; }
        public string Commitment { get; set; }
        public string ActualShare { get; set; }
        public string CompetitorShare { get; set; }
    }
}
