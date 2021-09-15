using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities.Identity
{
    class SubMenu : BaseEntity
    {
        public int SmSeq { get; set; }
        public string SmName { get; set; }
        public string Url { get; set; }
        public string MhId { get; set; }
        [ForeignKey("MenuHead")]
        public MenuHead MenuHead { get; set; }

    }
}
