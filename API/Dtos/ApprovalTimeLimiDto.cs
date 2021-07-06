
namespace API.Dtos
{
    public class ApprovalTimeLimitDto
    {
        public int Id { get; set; }
        public int ApprovalAuthorityId { get; set; }
        public string ApprovalAuthorityName { get; set; }
        public string TimeLimit { get; set; }
        public string Remarks { get; set; }
        public int Status { get; set; }    
    }
}