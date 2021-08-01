﻿using Core.Entities;


namespace API.Dtos
{
    public class InvestmentAprProductsDto
    {
        public int Id { get; set; }
        public int InvestmentAprCmntId { get; set; }
        public InvestmentAprComment InvestmentAprComment { get; set; }
        public int ProductId { get; set; }
        public ProductInfo ProductInfo { get; set; }
    }
}
