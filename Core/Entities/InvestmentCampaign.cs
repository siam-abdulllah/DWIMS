using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentCampaign : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public int CampaignDtlId { get; set; }
        [ForeignKey("CampaignDtlId")]
        public CampaignDtl CampaignDtl { get; set; }
        public int InstitutionId { get; set; }
        [ForeignKey("InstitutionId")]
        public InstitutionInfo InstitutionInfo { get; set; }
        public int DoctorId { get; set; }
        [ForeignKey("DoctorId")]
        public DoctorInfo DoctorInfo { get; set; }
    }
}
