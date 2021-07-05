using Core.Entities;

namespace Core.Specifications
{
    public class SocietyWithFiltersForCountSpecificication : BaseSpecification<Society>
    {
        public SocietyWithFiltersForCountSpecificication(SocietySpecParams societyParrams)
            : base(x =>
                (string.IsNullOrEmpty(societyParrams.Search) || x.SocietyName.ToLower().Contains(societyParrams.Search))
            )
        {
        }
    }
}
