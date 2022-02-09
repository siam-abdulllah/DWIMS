using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentRcvPending : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }

        public int DonationId { get; set; }
        [ForeignKey("DonationId")]
        public Donation Donation { get; set; }
        public string DonationTo { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string SBU { get; set; }
        public string Name { get; set; }
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
        public bool Confirmation { get; set; }
        public DateTimeOffset? SubmissionDate { get; set; }
        public string ApprovedBy { get; set; }
        public DateTimeOffset? ApprovedDate { get; set; }
        public string DepotName { get; set; }

    }
}
