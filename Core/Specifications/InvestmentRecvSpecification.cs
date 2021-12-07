using Core.Entities;

namespace Core.Specifications
{
    public class InvestmentRecvSpecification : BaseSpecification<InvestmentRecv>
    {
        public InvestmentRecvSpecification(InvestmentRecvSpecParams parrams)
           : base(x =>
               (string.IsNullOrEmpty(parrams.Search) || x.Employee.SBU.ToLower().Contains(parrams.Search))
           )
        {
            AddInclude(x => x.Employee);
            AddOrderBy(x => x.SetOn);
            ApplyPaging(parrams.PageSize * (parrams.PageIndex - 1), parrams.PageSize);
        }

        public InvestmentRecvSpecification(int? id)
            : base(x => x.InvestmentInitId == id)
        {

        }
    }
}
