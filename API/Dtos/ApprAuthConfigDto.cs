using System;
namespace API.Dtos
{
    public class ApprAuthConfigDto
    {
        public int Id { get; set; }
        public int ApprovalAuthorityId { get; set; }
        public AuthEmployeeDto Employee { get; set; }
        public ApprovalAuthorityToReturnDto ApprovalAuthority { get; set; }

       public int EmployeeId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset SetOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}