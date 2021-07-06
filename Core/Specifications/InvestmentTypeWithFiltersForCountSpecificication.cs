using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentTypeWithFiltersForCountSpecificication : BaseSpecification<InvestmentType>
    {
        public InvestmentTypeWithFiltersForCountSpecificication(InvestmentTypeSpecParams approvalParrams)
          : base(x =>
              (string.IsNullOrEmpty(approvalParrams.Search) || x.Remarks.ToLower().Contains(approvalParrams.Search))
          )
        {
        }
    }
}
