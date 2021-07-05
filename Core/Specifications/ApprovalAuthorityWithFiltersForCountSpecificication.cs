using Core.Entities;

namespace Core.Specifications
{
    public class ApprovalAuthorityWithFiltersForCountSpecificication : BaseSpecification<ApprovalAuthority>
    {
         public ApprovalAuthorityWithFiltersForCountSpecificication(ApprovalAuthoritySpecParams ApprovalAuthorityParrams) 
            : base(x => 
                (string.IsNullOrEmpty(ApprovalAuthorityParrams.Search) || x.ApprovalAuthorityName.ToLower().Contains(ApprovalAuthorityParrams.Search))
            )
        {
        }
    }
}