using System;
using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class InvestmentNoSBUAprInsertDto
    {
        public InvestmentRecComment InvestmentRecComment { get; set; }
        public InvestmentApr InvestmentApr { get; set; }
        public List<InvestmentRecProducts> InvestmentRecProducts { get; set; }
    }
}
