using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecProductsDto
    {
        public int Id { get; set; }
        public int InvestmenRecCmntId { get; set; }
        public InvestmentRecComment InvestmentRecComment { get; set; }

        public int ProductId { get; set; }
        public ProductInfo ProductInfo { get; set; }
    }
}
