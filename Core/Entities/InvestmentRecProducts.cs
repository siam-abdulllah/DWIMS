using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class InvestmentRecProducts : BaseEntity
    {
        public int? InvestmentInitId { get; set; }
        [ForeignKey("InvestmentInitId")]
        public InvestmentInit InvestmentInit { get; set; }
        public string SBU { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public ProductInfo ProductInfo { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
