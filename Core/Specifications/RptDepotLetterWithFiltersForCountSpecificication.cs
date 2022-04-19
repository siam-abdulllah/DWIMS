using Core.Entities;


namespace Core.Specifications
{
    class RptDepotLetterWithFiltersForCountSpecificication : BaseSpecification<RptDepotLetter>
    {
        public RptDepotLetterWithFiltersForCountSpecificication(RptDepotLetterSpecParams rptParrams)
           : base(x =>
               (string.IsNullOrEmpty(rptParrams.Search) || x.DepotName.ToLower().Contains(rptParrams.Search))
           )
        {
        }
    }
}
