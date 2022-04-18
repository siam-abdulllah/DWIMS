using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentRapid : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }
        public string SBU { get; set; }
        public string Type { get; set; }
        public DateTime PropsalDate { get; set; }
        public string DonationTo { get; set; }
        public string Address { get; set; }
        public double ProposedAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }
        public string ChequeTitle { get; set; }

        public string DepotName{get;set;}
        public string DepotCode { get; set; }

        public int InitiatorId { get;set;}
     
        public int SubCampaignId { get;set;}
        public string SubCampaignName { get;set;}
    }
    [NotMapped]
    public class InvestmentRapidVM : InvestmentRapid
    {
        public string donationTypeName { get; set; }
        public string ApprovedStatus { get; set; }
        public int ApprovalAuthId { get; set; }
        public string Approval { get; set; }
    }
}
