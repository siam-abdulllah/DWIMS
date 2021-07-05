using Core.Entities.Identity;

namespace Core.Specifications
{
    public class UserWithFiltersForCountSpecificication : BaseSpecification<AppUser>
    {
         public UserWithFiltersForCountSpecificication(UserSpecParams postParrams) 
            : base(x => 
                (string.IsNullOrEmpty(postParrams.Search) || x.DisplayName.ToLower().Contains(postParrams.Search))
            )
        {
        }
    }
}