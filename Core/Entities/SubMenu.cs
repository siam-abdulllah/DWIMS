using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class SubMenu : BaseEntity
    {
        public int SubMenuSeq { get; set; }
        public string SubMenuName { get; set; }
        public string Url { get; set; }
        public int MenuHeadId { get; set; }

    }
}
