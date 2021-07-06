using Core.Entities;

namespace Core.Specifications
{
    public class ApprovalCeilingWithFiltersForCountSpecificication : BaseSpecification<ApprovalCeiling>
    {
        public ApprovalCeilingWithFiltersForCountSpecificication(ApprovalCeilingSpecParams approvalParrams)
          : base(x =>
              (string.IsNullOrEmpty(approvalParrams.Search) || x.Remarks.ToLower().Contains(approvalParrams.Search))
          )
        {
        }
    }
}
