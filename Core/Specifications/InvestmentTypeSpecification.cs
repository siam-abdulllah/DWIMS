using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentTypeSpecification : BaseSpecification<InvestmentType>
    {
        public InvestmentTypeSpecification(InvestmentTypeSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.InvesetmentTypeName.ToLower().Contains(approvalParrams.Search))
         )
        {
            AddOrderBy(x => x.SetOn);
            ApplyPaging(approvalParrams.PageSize * (approvalParrams.PageIndex - 1), approvalParrams.PageSize);
        }

        public InvestmentTypeSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.InvesetmentTypeName);
        }
    }
}
