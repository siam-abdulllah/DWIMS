using Core.Entities;


namespace Core.Specifications
{
    public class EmpPostSpecification : BaseSpecification<EmployeePosting>
    {
        public EmpPostSpecification(EmpPostSpecParams employeeparams)
 : base(x =>
     (string.IsNullOrEmpty(employeeparams.Search) || x.EmployeeId.ToLower().Contains(employeeparams.Search))
 )
        {
            AddOrderBy(x => x.SetOn);
            ApplyPaging(employeeparams.PageSize * (employeeparams.PageIndex - 1), employeeparams.PageSize);
        }

        public EmpPostSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.EmployeeId);
        }
    }
}

