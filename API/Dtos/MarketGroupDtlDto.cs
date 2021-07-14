using System;

namespace API.Dtos
{
    public class MarketGroupDtlDto
    {
        public int Id { get; set; }
        public int MstId { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string SBU { get; set; }
        public string Status { get; set; }

    }
}