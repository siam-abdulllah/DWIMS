using Core.Entities;
using System;
using System.Globalization;

namespace Core.Specifications
{
    public class YearlyBudgetSpecificiation : BaseSpecification<YearlyBudget>
    {
        public YearlyBudgetSpecificiation(YearlyBudgetSpecParams sbuParrams)
       : base(x =>
           (string.IsNullOrEmpty(sbuParrams.Search) || x.Year.ToString().ToLower().Contains(sbuParrams.Search))
       )
        {
           ;
            AddOrderBy(x => x.FromDate);
            ApplyPaging(sbuParrams.PageSize * (sbuParrams.PageIndex - 1), sbuParrams.PageSize);
        }

        public YearlyBudgetSpecificiation(int id)
            : base(x => x.Id == id)
        {
        } 
        public YearlyBudgetSpecificiation(string year)
            : base(x => x.Year == year)
        {
        }
        
    }
}
