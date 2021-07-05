namespace Core.Entities
{
    public class ApprAuthConfig: BaseEntity
    {
        public string Status { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public int ApprovalAuthorityId { get; set; }
        public ApprovalAuthority ApprovalAuthority { get; set; }
    }
}