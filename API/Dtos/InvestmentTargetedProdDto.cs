using System;

namespace API.Dtos
{
    public class InvestmentTargetedProdDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }

        
        public InvestmentInitDto InvestmentInitDto { get; set; }
        public int ProductId { get; set; }
        public string SBU { get; set; }
        public ProductDto ProductDto { get; set; }
    }
}