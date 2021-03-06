using Core.Entities;

namespace Core.Specifications
{
    public class MarketGroupDtlSpecification : BaseSpecification<MarketGroupDtl>
    {

        public MarketGroupDtlSpecification(MarketGroupDtlSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.MarketCode.ToLower().Contains(parrams.Search))
           )
        {
            
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

        public MarketGroupDtlSpecification(int id)
            : base(x => x.MstId == id && x.Status== "Active")
        {
        } public MarketGroupDtlSpecification(int? id)
            : base(x => x.MstId == id && x.Status== "Active")
        {
        }
        public MarketGroupDtlSpecification(int mstId,string marketCode,string status)
            : base(x => x.MstId == mstId &&  x.MarketCode == marketCode && x.Status==status)
        {
        }
      

    }
}
