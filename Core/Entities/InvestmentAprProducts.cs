using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentAprProducts : BaseEntity
    {
        public int InvestmentAprId { get; set; }
        [ForeignKey("InvestmentAprId")]
        public InvestmentApr InvestmentApr { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
    }
}
