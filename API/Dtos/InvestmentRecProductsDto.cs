using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecProductsDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }

        public int ProductId { get; set; }
        public ProductInfo ProductInfo { get; set; }
    }
}
