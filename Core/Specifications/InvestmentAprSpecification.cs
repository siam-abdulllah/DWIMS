using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentAprSpecification : BaseSpecification<InvestmentApr>
    {

        public InvestmentAprSpecification(InvestmentAprSpecParams parrams)
           : base(x =>
               //(string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
               (string.IsNullOrEmpty(parrams.Search) )
           )
        {
            
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }

        public InvestmentAprSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {

        }
        public InvestmentAprSpecification(string donationType)
           : base(x => x.InvestmentInit.DonationType == donationType)
        {
            AddInclude(x => x.InvestmentInit);

        }
        //public InvestmentAprSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentAprSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
