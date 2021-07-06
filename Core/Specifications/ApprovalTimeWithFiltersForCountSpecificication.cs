using Core.Entities;

namespace Core.Specifications
{
    public class ApprovalTimeWithFiltersForCountSpecificication : BaseSpecification<ApprovalTimeLimit>
    {
         public ApprovalTimeWithFiltersForCountSpecificication(ApprovalTimeSpecParams approvalParrams) 
            : base(x => 
                (string.IsNullOrEmpty(approvalParrams.Search) || x.Remarks.ToLower().Contains(approvalParrams.Search))
            )
        {
        }
    }
}