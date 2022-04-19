using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentRecCommentWithFiltersForCountSpecificication : BaseSpecification<InvestmentRecComment>
    {
         public InvestmentRecCommentWithFiltersForCountSpecificication(InvestmentRecCommentSpecParams InvestmentRecCommentParrams) 
            : base(x => 
                (string.IsNullOrEmpty(InvestmentRecCommentParrams.Search) || x.Employee.EmployeeName.ToLower().Contains(InvestmentRecCommentParrams.Search))
            )
        {
        } 
        public InvestmentRecCommentWithFiltersForCountSpecificication(int id)
             : base(x => x.Id == id)
        {
            AddInclude(x => x.Employee);
        }
    }
}