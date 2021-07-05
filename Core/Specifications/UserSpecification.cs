using Core.Entities.Identity;

namespace Core.Specifications
{
     public class UserSpecification : BaseSpecification<AppUser>
    {
        public UserSpecification(UserSpecParams postParrams) 
            : base(x => 
                (string.IsNullOrEmpty(postParrams.Search) || x.DisplayName.ToLower().Contains(postParrams.Search))
            )
        {
        
            AddOrderBy(x => x.DisplayName);
            ApplyPaging(postParrams.PageSize * (postParrams.PageIndex - 1), postParrams.PageSize);
          
        }

        public UserSpecification(string id) 
            : base(x => x.Id == id)
        {
            
        }
    }
}