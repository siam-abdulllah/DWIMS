using Core.Entities;

namespace Core.Specifications
{
    public class MarketGroupSpecification : BaseSpecification<MarketGroupMst>
    {

        public MarketGroupSpecification(MarketGroupSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.GroupName.ToLower().Contains(parrams.Search))
           )
        {
            AddInclude(x => x.Employee);
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

        public MarketGroupSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.MarketGroupDtls);
        }
      

    }
}
