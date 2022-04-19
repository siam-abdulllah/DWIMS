using System;

namespace API.Dtos
{
    public class CampaignDtlProductDto
    {
        public int Id { get; set; }
        public int DtlId { get; set; }
        public int ProductId { get; set; }
        public ProductDto ProductInfo { get; set; }

    }
}