using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities.Identity
{
    public class AppUser : IdentityUser
    {
        public int EmployeeId { get; set; }
        public string EmployeeSAPCode { get; set; }
        public string DisplayName { get; set; }
        public Address Address { get; set; }  // 1 to 1 relation

    }
}