using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class RptInvestmentSummary : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string DonationTo { get; set; }
        public string Name { get; set; }
        public double? ProposedAmount { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public string InvStatus { get; set; }
        public int InvStatusCount { get; set; }
        public string EmployeeName { get; set; }
        public string ReceiveStatus { get; set; }
        public string ReceiveBy { get; set; }
        public string ApprovedBy { get; set; }
        public string MarketName { get; set; }
        public string DepotName { get; set; }
        public string SBUName { get; set; }
        public string ProposeFor { get; set; }
        public string PaymentMethod { get; set; }
        public bool Confirmation { get; set; }
        public int? DId { get; set; }
    }

    public class SystemSummary : BaseEntity
    {
        public string Phase { get; set; }
        public int? SBUA { get; set; }
        public int? SBUB { get; set; }
        public int? SBUC { get; set; }
        public int? SBUD { get; set; }
        public int? SBUE { get; set; }
        public int? SBUN { get; set; }
    }
}
