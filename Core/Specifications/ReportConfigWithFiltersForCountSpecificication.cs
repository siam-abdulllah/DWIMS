using Core.Entities;

namespace Core.Specifications
{
    public class ReportConfigWithFiltersForCountSpecificication : BaseSpecification<ReportConfig>
    {
        public ReportConfigWithFiltersForCountSpecificication(ReportConfigSpecParams apprAuthConfigParrams)
           : base(x =>
               (string.IsNullOrEmpty(apprAuthConfigParrams.Search) || x.ReportName.Contains(apprAuthConfigParrams.Search))
           )
        {
        }
    }
}
