using System.ComponentModel.DataAnnotations.Schema;
namespace Core.Entities
{
    public class CampaignDtlProduct:BaseEntity
    {
        public int DtlId { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
        
    }
}