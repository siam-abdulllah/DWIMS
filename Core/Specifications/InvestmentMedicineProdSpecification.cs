using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentMedicineProdSpecification : BaseSpecification<InvestmentMedicineProd>
    {

        public InvestmentMedicineProdSpecification(InvestmentMedicineProdSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) )
           )
        {
            AddInclude(x => x.MedicineProduct);
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

        public InvestmentMedicineProdSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {
            AddInclude(x => x.MedicineProduct);
        } 
        //public InvestmentMedicineProdSpecification(int id,string sbu)
        //    : base(x => x.InvestmentInitId == id && (x.ProductInfo.SBU==sbu || sbu=="All"))
        //{
        //    AddInclude(x => x.ProductInfo);
        //} 
        public InvestmentMedicineProdSpecification(int id,int prodId)
            : base(x => x.InvestmentInitId == id && x.ProductId==prodId)
        {
            //AddInclude(x => x.ProductInfo);
        }
        //public InvestmentTargetedProdSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentTargetedProdSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
