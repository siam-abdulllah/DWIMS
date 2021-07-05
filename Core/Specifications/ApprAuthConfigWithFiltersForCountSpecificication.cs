using Core.Entities;

namespace Core.Specifications
{
    public class ApprAuthConfigWithFiltersForCountSpecificication : BaseSpecification<ApprAuthConfig>
    {
         public ApprAuthConfigWithFiltersForCountSpecificication(ApprAuthConfigSpecParams apprAuthConfigParrams) 
            : base(x => 
                (string.IsNullOrEmpty(apprAuthConfigParrams.Search) || x.ApprovalAuthority.ApprovalAuthorityName.ToLower().Contains(apprAuthConfigParrams.Search))
            )
        {
        }
    }
}