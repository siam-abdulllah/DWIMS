using Core.Entities;

namespace Core.Specifications
{
    public class SubMenuWithFiltersForCountSpecificication : BaseSpecification<SubMenu>
    {
         public SubMenuWithFiltersForCountSpecificication(SubMenuSpecParams SubMenuParrams) 
            : base(x => 
                (string.IsNullOrEmpty(SubMenuParrams.Search) || x.SubMenuName.ToLower().Contains(SubMenuParrams.Search))
            )
        {
        }public SubMenuWithFiltersForCountSpecificication(int menuHeadId) 
            : base(x => 
                (x.MenuHeadId==menuHeadId)
            )
        {
        }
    }
}