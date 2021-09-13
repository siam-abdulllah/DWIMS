using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecProductsDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }

        public string SBU { get; set; }
        public int ProductId { get; set; }
        public ProductInfo ProductInfo { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
    }
}
