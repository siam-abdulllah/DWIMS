using Core.Entities;
namespace Core.Specifications
{
    public class RptDepotLetterSpecification : BaseSpecification<RptDepotLetter>
    {
        public RptDepotLetterSpecification(RptDepotLetterSpecParams rptParrams)
           : base(x =>
               (string.IsNullOrEmpty(rptParrams.Search) || x.DepotName.ToLower().Contains(rptParrams.Search))
           )
        {
            AddOrderBy(x => x.SetOn);
            ApplyPaging(rptParrams.PageSize * (rptParrams.PageIndex - 1), rptParrams.PageSize);
        }
    }
}
