using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentRecSpecification : BaseSpecification<InvestmentRec>
    {
        private int investmentInitId;

        public InvestmentRecSpecification(InvestmentRecSpecParams parrams)
           : base(x =>
               //(string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
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

        public InvestmentRecSpecification(int investmentInitId)
            : base(x => x.InvestmentInitId == investmentInitId)
        {
        }

        public InvestmentRecSpecification(int? id, int empId)
            : base(x => x.InvestmentInitId == id)
        {

        }
        public InvestmentRecSpecification(int empId, string recStatus)
            : base(x => x.InvestmentInit.EmployeeId == empId)
        {

        }
        public InvestmentRecSpecification(int id, int empId)
            : base(x => x.InvestmentInitId == id && x.EmployeeId == empId)
        {

        }
        //public InvestmentRecSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentRecSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
