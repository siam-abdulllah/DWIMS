using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentInitSpecification : BaseSpecification<InvestmentInit>
    {

        public InvestmentInitSpecification(InvestmentInitSpecParams parrams)
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

        //public InvestmentInitSpecification(int id)
        //    : base(x => x.InvestmentInitId == id)
        //{
            
        //}
        //public InvestmentInitSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentInitSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
