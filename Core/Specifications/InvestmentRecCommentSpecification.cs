using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentRecCommentSpecification : BaseSpecification<InvestmentRecComment>
    {

        public InvestmentRecCommentSpecification(InvestmentRecCommentSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
           )
        {
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

        public InvestmentRecCommentSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {

        }       
        public InvestmentRecCommentSpecification(int id,int empId)
            : base(x => x.InvestmentInitId == id && x.EmployeeId==empId)
        {

        }
        public InvestmentRecCommentSpecification(int id,int priority,string completionStatus)
            : base(x => x.InvestmentInitId == id && x.Priority== priority)
        {

        }
        //public InvestmentRecCommentSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentRecCommentSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
