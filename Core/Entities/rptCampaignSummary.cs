using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class RptCampaignSummary : BaseEntity
    {
        public string CampaignName { get; set; }
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string SBUName { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }  
        public int? InstitutionId { get; set; }
        public string InstitutionName { get; set; }
        public int? DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string RecStatus { get; set; }
        public double? Total { get; set; }
        public double? ProposedAmount { get; set; }    
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string PaymentFreq {get; set; }
        public string DepotName {get; set; }
        public string ApprovedBy {get; set; }
    }
}
