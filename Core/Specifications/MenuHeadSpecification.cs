using Core.Entities;
using System;
using System.Globalization;

namespace Core.Specifications
{
    public class MenuHeadSpecification : BaseSpecification<MenuHead>
    {
        public MenuHeadSpecification(MenuHeadSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.MenuHeadName.ToLower().Contains(approvalParrams.Search))
         )
        {
           
        }

        public MenuHeadSpecification(int id)
            : base(x => x.Id == id)
        {
            
        }
       
    }
}
