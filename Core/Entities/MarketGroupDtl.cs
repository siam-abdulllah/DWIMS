using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class MarketGroupDtl:BaseEntity
    {
        public int MstId { get; set; }
        public string MarketCode { get; set; }
        public string  MarketName { get; set; }
        public string Status { get; set; }
    }
}
