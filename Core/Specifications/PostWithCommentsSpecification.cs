using Core.Entities;

namespace Core.Specifications
{
  
     public class PostWithCommentsSpecification : BaseSpecification<Post>
    {
        public PostWithCommentsSpecification(PostSpecParams postParrams) 
            : base(x => 
                (string.IsNullOrEmpty(postParrams.Search) || x.PostTitle.ToLower().Contains(postParrams.Search))
            )
        {
            AddInclude(x => x.PostComments);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(postParrams.PageSize * (postParrams.PageIndex - 1), postParrams.PageSize);

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

        public PostWithCommentsSpecification(int id) 
            : base(x => x.Id == id)
        {
            AddInclude(x => x.PostComments);
        }
    }
}