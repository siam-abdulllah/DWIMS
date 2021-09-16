using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class MenuConf : BaseEntity
    {
        
      
        public int SubMenuId { get; set; }
        public int RoleId { get; set; }

    }
}
