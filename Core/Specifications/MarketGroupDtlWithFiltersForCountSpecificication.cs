using Core.Entities;

namespace Core.Specifications
{
    public class MarketGroupDtlWithFiltersForCountSpecificication : BaseSpecification<MarketGroupDtl>
    {
         public MarketGroupDtlWithFiltersForCountSpecificication(MarketGroupDtlSpecParams marketGroupParrams) 
            : base(x => 
                (string.IsNullOrEmpty(marketGroupParrams.Search) || x.MarketCode.ToLower().Contains(marketGroupParrams.Search))
            )
        {
        } 
        public MarketGroupDtlWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
        }
    }
}