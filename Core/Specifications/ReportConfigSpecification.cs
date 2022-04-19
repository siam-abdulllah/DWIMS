using Core.Entities;

namespace Core.Specifications
{
    public class ReportConfigSpecification : BaseSpecification<ReportConfig>
    {
        public ReportConfigSpecification(ReportConfigSpecParams parrams)
          : base(x =>
              (string.IsNullOrEmpty(parrams.Search) || x.ReportName.ToLower().Contains(parrams.Search))
          )
        {
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
    }
}
