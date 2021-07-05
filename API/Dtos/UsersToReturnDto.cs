using System.Collections.Generic;
using System.Security.Claims;

namespace API.Dtos
{
    public class UsersToReturnDto
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailConfirmed { get; set; }
        public IList<string> Roles  { get; set; }
        public IList<Claim> Claims  { get; set; }
    }
}