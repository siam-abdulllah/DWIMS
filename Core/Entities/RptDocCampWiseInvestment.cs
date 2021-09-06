using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class RptDocCampWiseInvestment : BaseEntity
    {
        [StringLength(maximumLength: 50)]
        public string SBUName { get; set; }
        [StringLength(maximumLength: 10)]
        public string SBUCode { get; set; }
        [StringLength(maximumLength: 10)]
        public string MarketCode { get; set; }
        [StringLength(maximumLength: 70)]
        public string MarketName { get; set; }
        [StringLength(maximumLength: 10)]
        public string TerritoryCode { get; set; }
        [StringLength(maximumLength: 70)]
        public string TerritoryName { get; set; }
        [StringLength(maximumLength: 10)]
        public string RegionCode { get; set; }
        [StringLength(maximumLength: 70)]
        public string RegionName { get; set; }
        [StringLength(maximumLength: 10)]
        public string DivisionCode { get; set; }
        [StringLength(maximumLength: 50)]
        public string DivisionName { get; set; }
        [StringLength(maximumLength: 10)]
        public string ZoneCode { get; set; }
        [StringLength(maximumLength: 50)]
        public string ZoneName { get; set; }
        [StringLength(maximumLength: 10)]
        public string DoctorId { get; set; }
        [StringLength(maximumLength: 150)]
        public string DoctorName { get; set; }
        [StringLength(maximumLength: 30)]
        public string DoctorCategory { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double InvestmentPresent { get; set; }
        public double InvestmentPast { get; set; }
        [StringLength(maximumLength: 50)]
        public string Brand { get; set; }
        [StringLength(maximumLength: 50)]
        public string Campaign { get; set; }
        [StringLength(maximumLength: 50)]
        public string SubCampaign { get; set; }
        [StringLength(maximumLength: 50)]
        public string Commitment { get; set; }
        [StringLength(maximumLength: 10)]
        public string ActualShareBrand { get; set; }
        [StringLength(maximumLength: 10)]
        public string ActualShare { get; set; }
        [StringLength(maximumLength: 10)]
        public string CompetitorShare { get; set; }
        [StringLength(maximumLength: 10)]
        public string NoOfPresc { get; set; }
        [StringLength(maximumLength: 10)]
        public string NoOfPatient { get; set; }

    }
}
