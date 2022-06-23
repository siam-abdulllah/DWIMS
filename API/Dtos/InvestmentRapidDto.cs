using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRapidDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }
        public string SBU { get; set; }
        public string SbuName { get; set; }
        public string Type { get; set; }
        public DateTime PropsalDate { get; set; }
        public string ProposalDateStr { get; set; }
        public string DonationTo { get; set; }
        //public string Address { get; set; }
        public double ProposedAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string Remarks { get; set; }
        public string ChequeTitle { get; set; }
        public string DepotName { get; set; }
        public string DepotCode { get; set; }
       // public int SubCampaignId { get; set; }
       // public string SubCampaignName { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime SetOn { get; set; }
        public int InitiatorId { get; set; }
        public int ApproverId { get; set; }
        public string ApprovedStatus { get; set; }
        public string Approval { get; set; }
        public List<InvestmentMedicineProd> InvestmentMedicineProd { get; set; }
        public List<InvestmentRecProducts> InvestmentRecProducts { get; set; }
        public InvestmentDoctor InvestmentDoctor { get; set; }
        public InvestmentInstitution InvestmentInstitution { get; set; }
        public InvestmentCampaign InvestmentCampaign { get; set; }
        public InvestmentBcds InvestmentBcds { get; set; }
        public InvestmentSociety InvestmentSociety { get; set; }
        public InvestmentOther InvestmentOther { get; set; }
     
    }
}
