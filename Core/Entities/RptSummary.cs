using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class RptSummary : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public int? DId { get; set; }
        public string Name { get; set; }
        public string EmployeeName { get; set; }
        public string SBUName { get; set; }
        public string MarketName { get; set; }
        public string TerritoryName { get; set; }
        public string RegionName { get; set; }
        public string ZoneName { get; set; }
        public double? ProposedAmount { get; set; }
        public string InvStatus { get; set; }
        public string ApprovedBy { get; set; }
        public string DonationTypeName { get; set; }
        public string DonationTo { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentFreq { get; set; }
        public string DepotName { get; set; }
        public string ReceiveStatus { get; set; }
        public string ReceiveBy { get; set; }
        public string ProposeFor { get; set; }
        public bool Confirmation { get; set; }
        public string PaymentRefNo { get; set; }
        public string SAPRefNo { get; set; }
    }
}
