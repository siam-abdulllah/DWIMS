using Core.Entities;

namespace Core.Specifications
{
    public class CampaignDtlProductWithFiltersForCountSpecificication : BaseSpecification<CampaignDtlProduct>
    {
         public CampaignDtlProductWithFiltersForCountSpecificication(CampaignDtlProductSpecParams campaignDtlParrams) 
            : base(x => 
                (string.IsNullOrEmpty(campaignDtlParrams.Search) )
            )
        {
        } 
        public CampaignDtlProductWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
        }
    }
}