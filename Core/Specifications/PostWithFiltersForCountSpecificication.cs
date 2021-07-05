using Core.Entities;
namespace Core.Specifications
{
    public class PostWithFiltersForCountSpecificication : BaseSpecification<Post>
    {
         public PostWithFiltersForCountSpecificication(PostSpecParams postParrams) 
            : base(x => 
                (string.IsNullOrEmpty(postParrams.Search) || x.PostTitle.ToLower().Contains(postParrams.Search))
            )
        {
        }
    }
}