using Core.Entities;
namespace Core.Specifications
{
    public class SubCampaignWithFiltersForCountSpecificication : BaseSpecification<SubCampaign>
    {
         public SubCampaignWithFiltersForCountSpecificication(SubCampaignSpecParams subCampaignParrams) 
            : base(x => 
                (string.IsNullOrEmpty(subCampaignParrams.Search) || x.SubCampaignName.ToLower().Contains(subCampaignParrams.Search))
            )
        {
        }
    }
}