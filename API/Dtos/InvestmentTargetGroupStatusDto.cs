using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvestmentTargetGroupStatusDto
    {
        public int InvestmentInitId { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string MarketGroupName { get; set; }
        public Int32 Priority { get; set; }
        public string RecStatus { get; set; }
    }
}
