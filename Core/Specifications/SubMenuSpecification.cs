using Core.Entities;
using System;
using System.Globalization;

namespace Core.Specifications
{
    public class SubMenuSpecification : BaseSpecification<SubMenu>
    {
        public SubMenuSpecification(SubMenuSpecParams approvalParrams)
         : base(x =>
             (string.IsNullOrEmpty(approvalParrams.Search) || x.SubMenuName.ToLower().Contains(approvalParrams.Search))
         )
        {
           
        }

        public SubMenuSpecification(int menuHeadId)
            : base(x => x.MenuHeadId == menuHeadId)
        {
            
        }
       
    }
}
