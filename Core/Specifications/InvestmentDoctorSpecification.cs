﻿using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentDoctorSpecification : BaseSpecification<InvestmentDoctor>
    {

        public InvestmentDoctorSpecification(InvestmentDoctorSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.DoctorInfo.DoctorName.ToLower().Contains(parrams.Search))
           )
        {
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

        public InvestmentDoctorSpecification(int id)
            : base(x => x.InvestmentInitId == id)
        {
            AddInclude(x => x.DoctorInfo);
            AddInclude(x => x.InstitutionInfo);
        } 
        //public InvestmentDoctorSpecification(string donationType)
        //    : base(x => x.InvestmentInit.DonationType == donationType)
        //{
        //    AddInclude(x => x.InvestmentInit);
        //    AddInclude(x => x.DoctorInfo);
        //    AddInclude(x => x.InstitutionInfo);
        //}
        //public InvestmentDoctorSpecification(int employeeId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.Status == status)
        //{

        //}
        //public InvestmentDoctorSpecification(int employeeId,int authId,string status)
        //    : base(x => x.EmployeeId == employeeId && x.ApprovalAuthorityId == authId && x.Status == status)
        //{

        //}

    }
}
