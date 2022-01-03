using System;

namespace API.Dtos
{
    public class CampaignMstDto
    {
        public int Id { get; set; }
        public string CampaignNo { get; set; }
        public string CampaignName { get; set; }
        public string SBU { get; set; }
        public string BrandCode { get; set; }
        public int? EmployeeId { get; set; }

    }
}