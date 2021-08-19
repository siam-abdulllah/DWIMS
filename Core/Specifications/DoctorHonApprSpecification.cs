using Core.Entities;

namespace Core.Specifications
{
    public class DoctorHonApprSpecification : BaseSpecification<DoctorHonAppr>
    {
        public DoctorHonApprSpecification(DoctorHonApprSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.HonMonth.ToLower().Contains(approvalParrams.Search))
         )
        {
            AddInclude(x => x.DoctorInfo);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(approvalParrams.PageSize * (approvalParrams.PageIndex - 1), approvalParrams.PageSize);
        }

        public DoctorHonApprSpecification(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.DoctorInfo);
        }public DoctorHonApprSpecification(string honMonth)
            : base(x => x.HonMonth == honMonth)
        {
            AddInclude(x => x.DoctorInfo);
        }
    }
}
