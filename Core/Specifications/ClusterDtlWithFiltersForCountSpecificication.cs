using Core.Entities;

namespace Core.Specifications
{
    public class ClusterDtlWithFiltersForCountSpecificication : BaseSpecification<ClusterDtl>
    {
         public ClusterDtlWithFiltersForCountSpecificication(ClusterDtlSpecParams campaignDtlParrams) 
            : base(x => 
                (string.IsNullOrEmpty(campaignDtlParrams.Search) || x.RegionName.ToLower().Contains(campaignDtlParrams.Search))
            )
        {
        } 
        public ClusterDtlWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
           
        }
    }
}