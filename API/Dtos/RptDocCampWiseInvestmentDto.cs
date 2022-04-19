using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RptDocCampWiseInvestmentDto
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
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string DoctorCategory { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double InvestmentPresent { get; set; }
        public double InvestmentPast { get; set; }
        public string Brand { get; set; }
        public string Campaign { get; set; }
        public string SubCampaign { get; set; }
        public string Commitment { get; set; }
        public string ActualShareBrand { get; set; }
        public string ActualShare { get; set; }
        public string CompetitorShare { get; set; }
        public string NoOfPresc { get; set; }
        public string NoOfPatient { get; set; }
    }
}
