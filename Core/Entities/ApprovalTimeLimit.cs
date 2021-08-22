using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ApprovalTimeLimit:BaseEntity
    {
        public int ApprovalAuthorityId { get; set; }
        [ForeignKey("ApprovalAuthorityId")]
        public ApprovalAuthority ApprovalAuthority { get; set; }
        public int TimeLimit { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }    
    }
}