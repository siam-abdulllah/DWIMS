using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MenuConfig : BaseEntity
    {
        
      
        public int SubMenuId { get; set; }
        public string RoleId { get; set; }

    }
}
