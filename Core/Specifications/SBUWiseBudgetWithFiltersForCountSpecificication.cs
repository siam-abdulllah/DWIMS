using Core.Entities;
using System;

namespace Core.Specifications
{
    public class SBUWiseBudgetWithFiltersForCountSpecificication : BaseSpecification<SBUWiseBudget>
    {
        public SBUWiseBudgetWithFiltersForCountSpecificication(SBUWiseBudgetSpecParams sbuParrams)
        : base(x =>
            (string.IsNullOrEmpty(sbuParrams.Search) || x.Id.ToString().ToLower().Contains(sbuParrams.Search))
        )
        {
        }
        public SBUWiseBudgetWithFiltersForCountSpecificication(int id)
            : base(x => x.Id == id)
        {
            
        } 
        public SBUWiseBudgetWithFiltersForCountSpecificication(SBUWiseBudget sbuBdgt)
            : base(x => x.SBU == sbuBdgt.SBU && x.FromDate.Value.Date == sbuBdgt.FromDate.Value.Date && x.ToDate.Value.Date == sbuBdgt.ToDate.Value.Date)
        {
            
        }
        
        public SBUWiseBudgetWithFiltersForCountSpecificication(DateTimeOffset? dateCheck, string sbu)
            : base( x => x.SBU == sbu && (x.FromDate.Value.Date >= dateCheck || dateCheck <= x.ToDate.Value.Date))
        {

        }
    }
}
