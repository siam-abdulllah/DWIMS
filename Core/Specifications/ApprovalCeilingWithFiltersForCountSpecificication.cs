using Core.Entities;
using System;

namespace Core.Specifications
{
    public class ApprovalCeilingWithFiltersForCountSpecificication : BaseSpecification<ApprovalCeiling>
    {
        public ApprovalCeilingWithFiltersForCountSpecificication(ApprovalCeilingSpecParams approvalParrams)
          : base(x =>
              (string.IsNullOrEmpty(approvalParrams.Search) || x.Remarks.ToLower().Contains(approvalParrams.Search))
          )
        {
        }

        public ApprovalCeilingWithFiltersForCountSpecificication(int? apprvAuthId, string donationType, DateTimeOffset? dateCheck)
         : base(x => x.ApprovalAuthorityId == apprvAuthId && x.DonationType == donationType && (x.InvestmentFrom.Value.Date >= dateCheck || dateCheck <= x.InvestmentTo.Value.Date))
        {
        }
        public ApprovalCeilingWithFiltersForCountSpecificication(int id,int? apprvAuthId, string donationType, DateTimeOffset? dateCheck)
         : base(x => x.ApprovalAuthorityId == apprvAuthId && !x.Id.Equals(id) && x.DonationType == donationType && (x.InvestmentFrom.Value.Date >= dateCheck || dateCheck <= x.InvestmentTo.Value.Date))
        {
        }
    }
}
