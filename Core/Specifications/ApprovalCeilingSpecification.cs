using Core.Entities;

namespace Core.Specifications
{
    public class ApprovalCeilingSpecification : BaseSpecification<ApprovalCeiling>
    {
        public ApprovalCeilingSpecification(ApprovalCeilingSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.Additional.ToLower().Contains(approvalParrams.Search))
         )
        {
            AddInclude(x => x.InvestmentType);
            AddInclude(x => x.ApprovalAuthority);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(approvalParrams.PageSize * (approvalParrams.PageIndex - 1), approvalParrams.PageSize);
        }

        public ApprovalCeilingSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Additional);
        }
    }
}
