using Core.Entities;

namespace Core.Specifications
{
    public class MarketGroupWithFiltersForCountSpecificication : BaseSpecification<MarketGroupMst>
    {
         public MarketGroupWithFiltersForCountSpecificication(MarketGroupSpecParams marketGroupParrams) 
            : base(x => 
                (string.IsNullOrEmpty(marketGroupParrams.Search) || x.GroupName.ToLower().Contains(marketGroupParrams.Search))
            )
        {
        }
    }
}