using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MarketInfo : BaseEntity
    {
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; } 
        
    }
}
