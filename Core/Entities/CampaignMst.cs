using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entities
{
    public class CampaignMst:BaseEntity
    {
         //public int Id { get; set; }
        public string CampaignNo { get; set; }
        public string CampaignName { get; set; }
        public string SBU { get; set; }
        public int BrandId { get; set; }
        [ForeignKey("BrandId")]
        public BrandInfo BrandInfo { get; set; }
        [ForeignKey("MstId")]
        public virtual IEnumerable<CampaignDtl> CampaignDtls { get; set; }
        
        
        
    }
}