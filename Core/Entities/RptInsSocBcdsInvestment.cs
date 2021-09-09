using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public class RptInsSocBcdsInvestment : BaseEntity
    {
        [StringLength(maximumLength: 50)]
        public string SBUName { get; set; }
        [StringLength(maximumLength: 10)]
        public string SBUCode { get; set; }
        [StringLength(maximumLength: 10)]
        public string MarketGroupCode { get; set; }
        [StringLength(maximumLength: 70)]
        public string MarketGroupName { get; set; }
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
        public string InstitutionId { get; set; }
        [StringLength(maximumLength: 150)]
        public string InstitutionName { get; set; }
        [StringLength(maximumLength: 10)]
        public string SocietyId { get; set; }
        [StringLength(maximumLength: 150)]
        public string SocietyName { get; set; }
        [StringLength(maximumLength: 10)]
        public string BcdsId { get; set; }
        [StringLength(maximumLength: 150)]
        public string BcdsName { get; set; }
        [StringLength(maximumLength: 20)]
        public string DonationType { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double InvestedAmt { get; set; }
        [StringLength(maximumLength: 50)]
        public string Commitment { get; set; }
        [StringLength(maximumLength: 10)]
        public string ActualShare { get; set; }
        [StringLength(maximumLength: 10)]
        public string CompetitorShare { get; set; }
    }
}
