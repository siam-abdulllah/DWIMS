using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentAprProducts : BaseEntity
    {
        public int? InvestmentAprCmntId { get; set; }
        [ForeignKey("InvestmentAprCmntId")]
        public InvestmentAprComment InvestmentAprComment { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
    }
}
