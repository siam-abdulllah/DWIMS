using Core.Entities;

namespace Core.Specifications
{
    public class MarketGroupMstWithFiltersForCountSpecificication : BaseSpecification<MarketGroupMst>
    {
         public MarketGroupMstWithFiltersForCountSpecificication(MarketGroupMstSpecParams marketGroupParrams) 
            : base(x => 
                (string.IsNullOrEmpty(marketGroupParrams.Search) || x.GroupName.ToLower().Contains(marketGroupParrams.Search))
            )
        {
        } 
        public MarketGroupMstWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
            AddInclude(x => x.MarketGroupDtls);
        }
    }
}