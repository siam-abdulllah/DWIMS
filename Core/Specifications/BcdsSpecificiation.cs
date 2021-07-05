using Core.Entities;

namespace Core.Specifications
{
    public class BcdsSpecificiation : BaseSpecification<Bcds>
    {

        public BcdsSpecificiation(BcdsSpecParams bscdParrams)
           : base(x =>
               (string.IsNullOrEmpty(bscdParrams.Search) || x.BcdsName.ToLower().Contains(bscdParrams.Search))
           )
        {
            //AddInclude(x => x.BcdsAddress);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(bscdParrams.PageSize * (bscdParrams.PageIndex - 1), bscdParrams.PageSize);

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

        public BcdsSpecificiation(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.BcdsName);
        }

    }
}
