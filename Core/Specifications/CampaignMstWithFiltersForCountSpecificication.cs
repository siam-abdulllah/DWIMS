using Core.Entities;

namespace Core.Specifications
{
    public class CampaignMstWithFiltersForCountSpecificication : BaseSpecification<CampaignMst>
    {
         public CampaignMstWithFiltersForCountSpecificication(CampaignMstSpecParams campaignMstParrams) 
            : base(x => 
                (string.IsNullOrEmpty(campaignMstParrams.Search) || x.CampaignName.ToLower().Contains(campaignMstParrams.Search))
            )
        {
        } 
        public CampaignMstWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
            AddInclude(x => x.CampaignDtls);
        }
    }
}