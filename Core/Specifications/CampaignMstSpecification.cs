using Core.Entities;

namespace Core.Specifications
{
    public class CampaignMstSpecification : BaseSpecification<CampaignMst>
    {

        public CampaignMstSpecification(CampaignMstSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.CampaignName.ToLower().Contains(parrams.Search))
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

        public CampaignMstSpecification(int id)
            : base(x => x.Id == id )
        {
            AddInclude(x => x.CampaignDtls);
        }public CampaignMstSpecification()
            : base()
        {
            AddInclude(x => x.CampaignDtls);
           // AddInclude(x => x.CampaignDtls.SubCampaign);
        }
      

    }
}
