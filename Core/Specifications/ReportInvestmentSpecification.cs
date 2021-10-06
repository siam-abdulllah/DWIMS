using Core.Entities;
using System;
using System.Globalization;

namespace Core.Specifications
{
    public class ReportInvestmentSpecification : BaseSpecification<ReportInvestmentInfo>
    {

        public ReportInvestmentSpecification(ReportInvestmentSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.MarketCode.ToLower().Contains(parrams.Search))
           )
        {

            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);

            // if (!string.IsNullOrEmpty(postParrams.Sort))
            // {
            //     switch (postParrams.Sort)
            //     {
            //         case "priceAsc":
            //             AddOrderBy(p => p.Price);
            //             break;
            //         case "priceDesc":
            //             AddOrderByDescending(p => p.Price);
            //             break;
            //         default:
            //             AddOrderBy(n => n.Name);
            //             break;
            //     }
            // }
        }

        public ReportInvestmentSpecification(int id)
            : base(x => x.Id == id)
        {

        }
        public ReportInvestmentSpecification(string marketCode)
            : base(x => x.MarketCode == marketCode)
        {

        }
       
    }
}
