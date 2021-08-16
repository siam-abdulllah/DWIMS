using Core.Entities;

namespace Core.Specifications
{
    public class ReportInvestmentInfoSpecification : BaseSpecification<ReportInvestmentInfo>
    {

        public ReportInvestmentInfoSpecification(ReportInvestmentInfoSpecParams parrams)
           : base(x =>
               //(string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
               (string.IsNullOrEmpty(parrams.Search))
           )
        {
            AddInclude(x => x.InvestmentSociety);
            AddInclude(x => x.DoctorInfo);
            AddInclude(x => x.InstitutionInfo);
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

        public ReportInvestmentInfoSpecification(string fromDate, string toDate)
            : base(x => x.FromDate == fromDate && x.ToDate == toDate)
        {
            AddInclude(x => x.InvestmentSociety);
            AddInclude(x => x.DoctorInfo);
            AddInclude(x => x.InstitutionInfo);
            AddOrderBy(x => x.SetOn);  
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
