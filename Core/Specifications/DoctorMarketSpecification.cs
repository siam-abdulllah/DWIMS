using Core.Entities;

namespace Core.Specifications
{
    public class DoctorMarketSpecification : BaseSpecification<DoctorMarket>
    {
        public DoctorMarketSpecification(DoctorMarketSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.MarketCode.ToLower().Contains(approvalParrams.Search))
         )
        {
            AddOrderBy(x => x.SetOn);
            ApplyPaging(approvalParrams.PageSize * (approvalParrams.PageIndex - 1), approvalParrams.PageSize);
            
        }

        public DoctorMarketSpecification(string marketCode)
            : base(x => x.MarketCode == marketCode)
        {
           
        }
        //public DoctorMarketSpecification(string honMonth)
        //    : base(x => x.HonMonth == honMonth)
        //{
        //    AddInclude(x => x.DoctorInfo);
        //}
    }
}
