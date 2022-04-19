using Core.Entities;

namespace Core.Specifications
{
    public class SBUWithFiltersForCountSpecificication : BaseSpecification<SBU>
    {
        public SBUWithFiltersForCountSpecificication(SBUSpecParams sbuParrams)
         : base(x =>
             (string.IsNullOrEmpty(sbuParrams.Search) || x.SBUName.ToLower().Contains(sbuParrams.Search))
         )
        {
        }
    }
}
