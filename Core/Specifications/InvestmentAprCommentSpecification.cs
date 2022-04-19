using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentAprCommentSpecification : BaseSpecification<InvestmentAprComment>
    {

        public InvestmentAprCommentSpecification(InvestmentAprCommentSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
           )
        {
            AddInclude(x => x.Employee);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }

        public InvestmentAprCommentSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {

        }        public InvestmentAprCommentSpecification(int id,int empId)
            : base(x => x.InvestmentInitId == id && x.EmployeeId==empId)
        {

        }
        //public InvestmentAprCommentSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentAprCommentSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
