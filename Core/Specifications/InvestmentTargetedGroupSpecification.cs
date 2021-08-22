using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentTargetedGroupSpecification : BaseSpecification<InvestmentTargetedGroup>
    {

        public InvestmentTargetedGroupSpecification(InvestmentTargetedGroupSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.MarketCode.ToLower().Contains(parrams.Search))
           )
        {
            AddInclude(x => x.MarketGroupMst);
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

        public InvestmentTargetedGroupSpecification(int id)
            : base(x => x.InvestmentInitId == id )
        {
            AddInclude(x => x.MarketGroupMst);
        }public InvestmentTargetedGroupSpecification(int id,int? marketGroupMstId)
            : base(x => x.InvestmentInitId == id && x.MarketGroupMstId== marketGroupMstId)
        {
            AddInclude(x => x.MarketGroupMst);
        }
        //public InvestmentTargetedGroupSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentTargetedGroupSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
