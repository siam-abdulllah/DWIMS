using Core.Entities;

namespace Core.Specifications
{
    public class ClusterMstSpecification : BaseSpecification<ClusterMst>
    {

        public ClusterMstSpecification(ClusterMstSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.ClusterName.ToLower().Contains(parrams.Search))
           )
        {
            //AddInclude(x => x.Employee);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);

            // if (!string.IsNullOrEmpty(postParrams.Sort))
            // {
            //     switch (postParrams.Sort)
            //     {
            //         case "priceAsc":
            //             AddOrderBy(p => p.Price);
            //             break;
            //         case "priceDesc":
            //             AddOrderByDescending(p => p.Price);
            //             break;
            //         default:
            //             AddOrderBy(n => n.Name);
            //             break;
            //     }
            // }
        }

        public ClusterMstSpecification(int id)
            : base(x => x.Id == id )
        {
            AddInclude(x => x.ClusterDtls);
        }public ClusterMstSpecification()
            : base()
        {
            AddInclude(x => x.ClusterDtls);
           // AddInclude(x => x.ClusterDtls.SubCluster);
        }
      

    }
}
