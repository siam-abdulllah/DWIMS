using Core.Entities;

namespace Core.Specifications
{
    public class SBUWiseBudgetSpecificiation : BaseSpecification<SBUWiseBudget>
    {
        public SBUWiseBudgetSpecificiation(SBUWiseBudgetSpecParams sbuParrams)
       : base(x =>
           (string.IsNullOrEmpty(sbuParrams.Search) || x.SBU.ToString().ToLower().Contains(sbuParrams.Search))
       )
        {
            AddOrderBy(x => x.FromDate);
            ApplyPaging(sbuParrams.PageSize * (sbuParrams.PageIndex - 1), sbuParrams.PageSize);
        }

        public SBUWiseBudgetSpecificiation(int id)
            : base(x => x.Id == id)
        {
        }
    }
}
