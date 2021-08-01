using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentInitWithFiltersForCountSpecificication : BaseSpecification<InvestmentInit>
    {
         public InvestmentInitWithFiltersForCountSpecificication(InvestmentInitSpecParams InvestmentInitParrams) 
            : base(x => 
                (string.IsNullOrEmpty(InvestmentInitParrams.Search) || x.Employee.EmployeeName.ToLower().Contains(InvestmentInitParrams.Search))
            )
        {
        } 
        public InvestmentInitWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
            AddInclude(x => x.Employee);
        }
    }
}