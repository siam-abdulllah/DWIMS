using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InstitutionMarket : BaseEntity
    {
        public int InstitutionCode { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string Status { get; set; }
        public string SBU { get; set; }

    }
}
