using Core.Entities;

namespace Core.Specifications
{
    public class CampaignDtlProductSpecification : BaseSpecification<CampaignDtlProduct>
    {

        public CampaignDtlProductSpecification(CampaignDtlProductSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search))
           )
        {
            //AddInclude(SubCampaign);
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

        public CampaignDtlProductSpecification(CampaignDtlProductSpecParams parrams,int id)
            : base(x => x.DtlId == id )
        {
            AddInclude(x => x.ProductInfo);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }
        public CampaignDtlProductSpecification(int id)
            : base(x => x.DtlId == id )
        {
            AddInclude(x => x.ProductInfo);
            AddOrderBy(x => x.SetOn);
        }
       
      

    }
}
