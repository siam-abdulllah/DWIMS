using System;

namespace API.Dtos
{
    public class CampaignDtlDto
    {
        public int Id { get; set; }
        public int MstId { get; set; }
        public int SubCampaignId { get; set; }
        public SubCampaignToReturnDto SubCampaign { get; set; }
        public long Budget { get; set; }
        public DateTime SubCampStartDate { get; set; }
        public DateTime SubCampEndDate { get; set; }

    }
}