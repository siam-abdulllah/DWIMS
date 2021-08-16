using Core.Entities;

namespace Core.Specifications
{
    public class ProductSpecification : BaseSpecification<ProductInfo>
    {
        public ProductSpecification(ProductSpecParams productparams)
  : base(x =>
      (string.IsNullOrEmpty(productparams.Search) || x.ProductName.ToLower().Contains(productparams.Search))
  )
        {
            
            AddOrderBy(x => x.SetOn);
            ApplyPaging(productparams.PageSize * (productparams.PageIndex - 1), productparams.PageSize);
        }

        public ProductSpecification(int id)
            : base(x => x.Id == id)
        {
        } 
        public ProductSpecification(string sbu)
            : base(x => x.SBU == sbu)
        {
        }
    }
}
