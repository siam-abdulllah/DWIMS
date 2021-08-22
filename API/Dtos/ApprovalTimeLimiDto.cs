
namespace API.Dtos
{
    public class ApprovalTimeLimitDto
    {
        public int Id { get; set; }
        public int ApprovalAuthorityId { get; set; }
        public ApprovalAuthorityToReturnDto ApprovalAuthority { get; set; }
        public int TimeLimit { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }    
    }
}