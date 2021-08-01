using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecProductsDto
    {
        public int Id { get; set; }
        public int InvestmenRecId { get; set; }
        public InvestmentRec InvestmentRec { get; set; }

        public int ProductId { get; set; }
        public ProductInfo ProductInfo { get; set; }
    }
}
