using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Market:BaseEntity
    {
        public int MarketCode { get; set; }
        public int MarketName { get; set; }
    }
}
