using System;
using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecOwnSBUInsertDto
    {
        public InvestmentRecComment InvestmentRecComment { get; set; }
        public InvestmentRec InvestmentRec { get; set; }
        public List<InvestmentRecProducts> InvestmentRecProducts { get; set; }
    }
}
