using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentRecProducts : BaseEntity
    {
        public int InvestmenRecId { get; set; }
        [ForeignKey("InvestmenRecId")]
        public InvestmentRec InvestmentRec { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
    }
}
