using Core.Entities;

namespace Core.Specifications
{
    public class ReportInvestmentInfoSpecParamsWithFiltersForCountSpecificication : BaseSpecification<ReportInvestmentInfo>
    {
        public ReportInvestmentInfoSpecParamsWithFiltersForCountSpecificication(ReportInvestmentInfoSpecParams SpecParrams)
           : base(x =>
               (string.IsNullOrEmpty(SpecParrams.Search))
           )
        {
        }
        public ReportInvestmentInfoSpecParamsWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
            // AddInclude(x => x.Employee);
        }
    }
}
