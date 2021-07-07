using Core.Entities;

namespace Core.Specifications
{
    public class EmployeeSpecification : BaseSpecification<Employee>
    {
        public EmployeeSpecification(EmployeeSpecParams employeeparams)
  : base(x =>
      (string.IsNullOrEmpty(employeeparams.Search) || x.EmployeeName.ToLower().Contains(employeeparams.Search))
  )
        {
            
            AddOrderBy(x => x.SetOn);
            ApplyPaging(employeeparams.PageSize * (employeeparams.PageIndex - 1), employeeparams.PageSize);
        }

        public EmployeeSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.EmployeeName);
        }
    }
}
