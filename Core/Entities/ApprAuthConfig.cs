using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ApprAuthConfig: BaseEntity
    {
        public string Status { get; set; }

        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }

        public int ApprovalAuthorityId { get; set; }
        [ForeignKey("ApprovalAuthorityId")]
        public ApprovalAuthority ApprovalAuthority { get; set; }
    }
}