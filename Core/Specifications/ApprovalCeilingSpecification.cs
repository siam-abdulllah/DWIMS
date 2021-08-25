using Core.Entities;
using System;
using System.Globalization;

namespace Core.Specifications
{
    public class ApprovalCeilingSpecification : BaseSpecification<ApprovalCeiling>
    {
        public ApprovalCeilingSpecification(ApprovalCeilingSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.Additional.ToLower().Contains(approvalParrams.Search))
         )
        {
            AddInclude(x => x.ApprovalAuthority);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(approvalParrams.PageSize * (approvalParrams.PageIndex - 1), approvalParrams.PageSize);
        }

        public ApprovalCeilingSpecification(int id)
            : base(x => x.Id == id)
        {
            
        }
        public ApprovalCeilingSpecification(int approvalAuthorityId,string status,string date)
            : base(x => x.ApprovalAuthorityId == approvalAuthorityId && x.Status== status 
            && x.InvestmentFrom <= DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture)
            && x.InvestmentTo >= DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture))
        {
           
        }
    }
}
