using System;

namespace API.Dtos
{
    public class InvestmentCampaignDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        
        public int InstitutionId { get; set; }
        
        public InstitutionInfoDto InstitutionInfoDto { get; set; }
        public int DoctorId { get; set; }
        public DoctorInfoDto DoctorInfoDto { get; set; }

        public int CampaignDtlId { get; set; }
        public CampaignDtlDto CampaignDtlDto { get; set; }
    }
    }