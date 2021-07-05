using Core.Entities;

namespace Core.Specifications
{
    public class EmpPostWithFiltersForCountSpecificication : BaseSpecification<EmployeePosting>
    {
        public EmpPostWithFiltersForCountSpecificication(EmpPostSpecParams empParrams)
          : base(x =>
              (string.IsNullOrEmpty(empParrams.Search) || x.EmployeeId.ToLower().Contains(empParrams.Search))
          )
        {
        }
    }
}
