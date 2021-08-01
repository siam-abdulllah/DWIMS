using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentRecProducts : BaseEntity
    {
        public int? InvestmenRecCmntId { get; set; }
        [ForeignKey("InvestmenRecCmntId")]
        public InvestmentRecComment InvestmentRecComment { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
    }
}
