using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentAprProductSpecification : BaseSpecification<InvestmentAprProducts>
    {

        public InvestmentAprProductSpecification(InvestmentAprProductSpecParams parrams)
         : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.ProductInfo.ProductName.ToLower().Contains(parrams.Search))
           )
        {
            AddInclude(x => x.ProductInfo);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }

        public InvestmentAprProductSpecification(int id)
           : base(x => x.InvestmentInitId == id)
        {
            AddInclude(x => x.ProductInfo);
        }
        public InvestmentAprProductSpecification(int id, int prodId)
          : base(x => x.InvestmentInitId == id && x.ProductId == prodId)
        {
            //AddInclude(x => x.ProductInfo);
        }
        public InvestmentAprProductSpecification(int id, string sbu)
           : base(x => x.InvestmentInitId == id && x.ProductInfo.SBU == sbu)
        {
            AddInclude(x => x.ProductInfo);
        }
        //public InvestmentAprProductSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentAprProductSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
