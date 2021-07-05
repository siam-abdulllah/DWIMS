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
        public DateTime SubCampStartDate { get; set; }
        public DateTime SubCampEndDate { get; set; }
        
        [ForeignKey("DtlId")]
        public virtual IEnumerable<CampaignDtlProduct> CampaignDtlProducts { get; set; }
        
        
    }
}