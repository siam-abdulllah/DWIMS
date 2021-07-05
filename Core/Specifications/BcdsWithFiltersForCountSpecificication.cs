using Core.Entities;

namespace Core.Specifications
{
    public class BcdsWithFiltersForCountSpecificication : BaseSpecification<Bcds>
    {
         public BcdsWithFiltersForCountSpecificication(BcdsSpecParams bcdsParrams) 
            : base(x => 
                (string.IsNullOrEmpty(bcdsParrams.Search) || x.BcdsName.ToLower().Contains(bcdsParrams.Search))
            )
        {
        }
    }
}