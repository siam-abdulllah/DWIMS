using Core.Entities;

namespace Core.Specifications
{
    public class ClusterMstWithFiltersForCountSpecificication : BaseSpecification<ClusterMst>
    {
         public ClusterMstWithFiltersForCountSpecificication(ClusterMstSpecParams campaignMstParrams) 
            : base(x => 
                (string.IsNullOrEmpty(campaignMstParrams.Search) || x.ClusterName.ToLower().Contains(campaignMstParrams.Search))
            )
        {
        } 
        public ClusterMstWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
            AddInclude(x => x.ClusterDtls);
        }
    }
}