using Core.Entities;

namespace Core.Specifications
{
    public class EmployeeWithFiltersForCountSpecificication : BaseSpecification<Employee>
    {
        public EmployeeWithFiltersForCountSpecificication(EmployeeSpecParams empParrams)
           : base(x =>
               (string.IsNullOrEmpty(empParrams.Search) || x.EmployeeName.ToLower().Contains(empParrams.Search))
           )
        {
        }
    }
}
