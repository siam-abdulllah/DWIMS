using Core.Entities;


namespace API.Dtos
{
    public class InvestmentAprProductsDto
    {
        public int Id { get; set; }
        public int InvestmentAprId { get; set; }
        public InvestmentApr InvestmentApr { get; set; }
        public int ProductId { get; set; }
        public ProductInfo ProductInfo { get; set; }
    }
}
