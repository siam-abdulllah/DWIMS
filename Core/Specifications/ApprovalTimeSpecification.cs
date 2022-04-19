using Core.Entities;

namespace Core.Specifications
{
    public class ApprovalTimeSpecification : BaseSpecification<ApprovalTimeLimit>
    {
        public ApprovalTimeSpecification(ApprovalTimeSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.Remarks.ToLower().Contains(approvalParrams.Search))
         )
        {
            AddOrderBy(x => x.SetOn);
            ApplyPaging(approvalParrams.PageSize * (approvalParrams.PageIndex - 1), approvalParrams.PageSize);
        }

        public ApprovalTimeSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.Remarks);
        }
    }
}
