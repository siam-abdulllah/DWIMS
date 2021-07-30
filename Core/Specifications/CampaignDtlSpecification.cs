using Core.Entities;

namespace Core.Specifications
{
    public class CampaignDtlSpecification : BaseSpecification<CampaignDtl>
    {

        public CampaignDtlSpecification(CampaignDtlSpecParams parrams)
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

        public CampaignDtlSpecification(CampaignDtlSpecParams parrams,int id)
            : base(x => x.MstId == id )
        {
            AddInclude(x => x.SubCampaign);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }
       
      

    }
}
