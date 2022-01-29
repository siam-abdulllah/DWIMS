using System;
using System.Collections.Generic;
using Core.Entities;

namespace API.Dtos
{
    public class InvestmentRecOtherSBUInsertDto
    {
        public InvestmentRecComment InvestmentRecComment { get; set; }
        public List<InvestmentRecProducts> InvestmentRecProducts { get; set; }
    }
}
