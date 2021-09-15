using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Identity
{
    class MenuConf : BaseEntity
    {
        
        public string MhId { get; set; }
        [ForeignKey("MenuHead")]
        public MenuHead MenuHead { get; set; } 
        public int SmId { get; set; }
        [ForeignKey("MenuHead")]
        public SubMenu SubMenu { get; set; }
        public int RlId { get; set; }

    }
}
