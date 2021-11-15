using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentDetailTrackerSpecification : BaseSpecification<InvestmentDetailTracker>
    {

        public InvestmentDetailTrackerSpecification(InvestmentDetailTrackerSpecParams parrams)
           : base(x =>
               //(string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
               (string.IsNullOrEmpty(parrams.Search) )
           )
        {
            
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }

        public InvestmentDetailTrackerSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {

        } 
        public InvestmentDetailTrackerSpecification(int empId, string DetailTrackerStatus)
            : base(x => x.EmployeeId == empId && DetailTrackerStatus=="Approved")
        {

        }
        //public InvestmentDetailTrackerSpecification(string donationType)
        //   : base(x => x.InvestmentInit.DonationType == donationType)
        //{
        //    AddInclude(x => x.InvestmentInit);

        //}
        //public InvestmentDetailTrackerSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentDetailTrackerSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
