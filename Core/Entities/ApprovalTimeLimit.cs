namespace Core.Entities
{
    public class ApprovalTimeLimit:BaseEntity
    {
        public int ApprovalAuthorityId { get; set; }
        public ApprovalAuthority ApprovalAuthority { get; set; }
        public string TimeLimit { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }    
    }
}