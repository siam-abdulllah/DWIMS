using Core.Entities;

namespace Core.Specifications
{
    public class CampaignDtlWithFiltersForCountSpecificication : BaseSpecification<CampaignDtl>
    {
         public CampaignDtlWithFiltersForCountSpecificication(CampaignDtlSpecParams campaignDtlParrams) 
            : base(x => 
                (string.IsNullOrEmpty(campaignDtlParrams.Search) )
            )
        {
        } 
        public CampaignDtlWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
        }
    }
}