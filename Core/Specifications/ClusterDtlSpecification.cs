using Core.Entities;

namespace Core.Specifications
{
    public class ClusterDtlSpecification : BaseSpecification<ClusterDtl>
    {

        public ClusterDtlSpecification(ClusterDtlSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.RegionName.ToLower().Contains(parrams.Search))
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

        public ClusterDtlSpecification(int id)
            : base(x => x.MstId == id )
        {
            
        }public ClusterDtlSpecification()
            : base()
        {
          
        }
      

    }
}
