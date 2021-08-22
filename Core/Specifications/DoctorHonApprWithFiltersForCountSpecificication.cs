using Core.Entities;

namespace Core.Specifications
{
    public class DoctorHonApprWithFiltersForCountSpecificication : BaseSpecification<DoctorHonAppr>
    {
         public DoctorHonApprWithFiltersForCountSpecificication(DoctorHonApprSpecParams apprAuthConfigParrams) 
            : base(x => 
                (string.IsNullOrEmpty(apprAuthConfigParrams.Search) || x.HonMonth.ToLower().Contains(apprAuthConfigParrams.Search))
            )
        {
        }
    }
}