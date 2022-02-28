using System;
using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class InvestmenAprForOwnSBUInsert
    {
        public InvestmentRecComment InvestmentRecComment { get; set; }
        public InvestmentRecDepot investmentRecDepot { get; set; }
        public InvestmentApr InvestmentApr { get; set; }
        public List<InvestmentRecProducts> InvestmentRecProducts { get; set; }
    }
}
