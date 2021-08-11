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
        public DateTimeOffset SubCampStartDate { get; set; }
        public DateTimeOffset SubCampEndDate { get; set; }

    }
}