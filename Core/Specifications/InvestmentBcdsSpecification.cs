using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentBcdsSpecification : BaseSpecification<InvestmentBcds>
    {

        public InvestmentBcdsSpecification(InvestmentBcdsSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search))
           )
        {
            
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

        public InvestmentBcdsSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {
            AddInclude(x=>x.Bcds);
        }
        //public InvestmentDoctorSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentDoctorSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
