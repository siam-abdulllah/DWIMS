using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entities
{
    public class CampaignDtl:BaseEntity
    {
        //public int Id { get; set; }
        public int MstId { get; set; }
        public int SubCampaignId { get; set; }
        [ForeignKey("SubCampaignId")]
        public SubCampaign SubCampaign { get; set; }
        public long Budget { get; set; }
        public DateTimeOffset SubCampStartDate { get; set; }
        public DateTimeOffset SubCampEndDate { get; set; }
        
        [ForeignKey("DtlId")]
        public virtual IEnumerable<CampaignDtlProduct> CampaignDtlProducts { get; set; }
        
        
    }
    public class SubCampainDtl
    {
        public int SubCampId { get; set; }
        public string SBU { get; set; }
        public string SubCampaignName { get; set; }

    }
}