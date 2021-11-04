using Core.Entities;

namespace Core.Specifications
{
    public class ApprAuthConfigSpecification : BaseSpecification<ApprAuthConfig>
    {

        public ApprAuthConfigSpecification(ApprAuthConfigSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.ApprovalAuthority.ApprovalAuthorityName.ToLower().Contains(parrams.Search))
           )
        {
            AddInclude(x => x.ApprovalAuthority);
            AddInclude(x => x.Employee);
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

        public ApprAuthConfigSpecification()
             : base(x => x.Status == "A")
        {
            AddInclude(x => x.Employee);
        }
        public ApprAuthConfigSpecification(int authId)
            : base(x => x.ApprovalAuthorityId == authId && x.Status=="A")
        {
            AddInclude(x => x.Employee);
        }
        public ApprAuthConfigSpecification(int employeeId,string status)
            : base(x => x.EmployeeId == employeeId && x.Status == status)
        {
            AddInclude(x => x.ApprovalAuthority);
        }
        public ApprAuthConfigSpecification(int employeeId,int authId,string status)
            : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        {
            
        }

    }
}
