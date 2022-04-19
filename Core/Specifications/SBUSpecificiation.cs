using Core.Entities;

namespace Core.Specifications
{
    public class SBUSpecificiation : BaseSpecification<SBU>
    {

        public SBUSpecificiation(SBUSpecParams sbuParrams)
         : base(x =>
             (string.IsNullOrEmpty(sbuParrams.Search) || x.SBUName.ToLower().Contains(sbuParrams.Search))
         )
        {
            //AddInclude(x => x.BcdsAddress);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(sbuParrams.PageSize * (sbuParrams.PageIndex - 1), sbuParrams.PageSize);
        }

        public SBUSpecificiation(int id)
            : base(x => x.Id == id)
        {
            AddInclude(x => x.SBUName);
        }

    }
}
