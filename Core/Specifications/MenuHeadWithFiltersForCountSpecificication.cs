using Core.Entities;

namespace Core.Specifications
{
    public class MenuHeadWithFiltersForCountSpecificication : BaseSpecification<MenuHead>
    {
         public MenuHeadWithFiltersForCountSpecificication(MenuHeadSpecParams MenuHeadParrams) 
            : base(x => 
                (string.IsNullOrEmpty(MenuHeadParrams.Search) || x.MenuHeadName.ToLower().Contains(MenuHeadParrams.Search))
            )
        {
        }
    }
}