using Core.Entities;

namespace Core.Specifications
{
    public class MarketGroupMstSpecification : BaseSpecification<MarketGroupMst>
    {

        public MarketGroupMstSpecification(MarketGroupMstSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.GroupName.ToLower().Contains(parrams.Search))
           )
        {
            //AddInclude(x => x.Employee);
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

        //public MarketGroupMstSpecification(int empId)
        //    : base(x => x.EmployeeId == empId)
        //{
        //    AddInclude(x => x.MarketGroupDtls);
        //} 
        public MarketGroupMstSpecification(string marketCode)
            : base(x => x.MarketCode == marketCode)
        {
            AddInclude(x => x.MarketGroupDtls);
        } 
        public MarketGroupMstSpecification()
            : base(x => x.Status == "Active" )
        {
            AddInclude(x => x.MarketGroupDtls);
        }
      

    }
}
