using Core.Entities;

namespace Core.Specifications
{
    public class SocietySpecification : BaseSpecification<Society>
    {
        public SocietySpecification(SocietySpecParams societyParrams)
          : base(x =>
              (string.IsNullOrEmpty(societyParrams.Search) || x.SocietyName.ToLower().Contains(societyParrams.Search))
          )
        {
            //AddInclude(x => x.BcdsAddress);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(societyParrams.PageSize * (societyParrams.PageIndex - 1), societyParrams.PageSize);

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

        public SocietySpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.SocietyName);
        }
    }
}
