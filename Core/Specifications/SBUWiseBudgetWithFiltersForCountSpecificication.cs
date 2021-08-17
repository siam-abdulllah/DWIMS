using Core.Entities;

namespace Core.Specifications
{
    public class SBUWiseBudgetWithFiltersForCountSpecificication : BaseSpecification<SBUWiseBudget>
    {
        public SBUWiseBudgetWithFiltersForCountSpecificication(SBUWiseBudgetSpecParams sbuParrams)
        : base(x =>
            (string.IsNullOrEmpty(sbuParrams.Search) || x.Id.ToString().ToLower().Contains(sbuParrams.Search))
        )
        {
        }
        public SBUWiseBudgetWithFiltersForCountSpecificication(int id)
            : base(x => x.Id == id)
        {
            
        }
    }
}
