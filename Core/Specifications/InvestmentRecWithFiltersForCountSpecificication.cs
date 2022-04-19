using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentRecWithFiltersForCountSpecificication : BaseSpecification<InvestmentRec>
    {
         public InvestmentRecWithFiltersForCountSpecificication(InvestmentRecSpecParams InvestmentRecParrams) 
            : base(x => 
                (string.IsNullOrEmpty(InvestmentRecParrams.Search))
            )
        {
        } 
        public InvestmentRecWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
           // AddInclude(x => x.Employee);
        }
    }
}