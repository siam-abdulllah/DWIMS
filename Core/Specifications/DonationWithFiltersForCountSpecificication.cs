using Core.Entities;
namespace Core.Specifications
{
    public class DonationWithFiltersForCountSpecificication : BaseSpecification<Donation>
    {
         public DonationWithFiltersForCountSpecificication(DonationSpecParams donationParrams) 
            : base(x => 
                (string.IsNullOrEmpty(donationParrams.Search) || x.DonationTypeName.ToLower().Contains(donationParrams.Search))
            )
        {
        }
    }
}