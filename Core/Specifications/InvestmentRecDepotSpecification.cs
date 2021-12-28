using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentRecDepotSpecification : BaseSpecification<InvestmentRecDepot>
    {

        public InvestmentRecDepotSpecification(InvestmentRecDepotSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.DepotName.ToLower().Contains(parrams.Search))
           )
        {
            //AddInclude(x => x.DepotInfo);
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

        //public InvestmentRecDepotSpecification(int id)
        //    : base(x => x.InvestmentInitId == id)
        //{
        //    AddInclude(x => x.DepotInfo);
        //} 
        public InvestmentRecDepotSpecification(int? id, string depotCode)
            : base(x => x.InvestmentInitId == id && x.DepotCode == depotCode)
        {
            //AddInclude(x => x.DepotInfo);
        }
        //public InvestmentRecDepotSpecification(int id,int prodId,int empId)
        //    : base(x => x.InvestmentInitId == id && x.DepotId==prodId && x.EmployeeId==empId)
        //{
        //    //AddInclude(x => x.DepotInfo);
        //}
        //public InvestmentRecDepotSpecification(int id, string sbu)
        //   : base(x => x.InvestmentInitId == id && x.DepotInfo.SBU == sbu)
        //{
        //    AddInclude(x => x.DepotInfo);
        //}
        //public InvestmentRecDepotSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentRecDepotSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
