using Core.Entities;

namespace Core.Specifications
{
    public class ApprovalAuthoritySpecification : BaseSpecification<ApprovalAuthority>
    {

        public ApprovalAuthoritySpecification(ApprovalAuthoritySpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.ApprovalAuthorityName.ToLower().Contains(parrams.Search))
           )
        {
            //AddInclude(x => x.ApprovalAuthorityAddress);
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

        public ApprovalAuthoritySpecification(string status)
            : base(x => x.Status == status)
        {
        }public ApprovalAuthoritySpecification(int id)
            : base(x => x.Id == id)
        {
        }

    }
}
