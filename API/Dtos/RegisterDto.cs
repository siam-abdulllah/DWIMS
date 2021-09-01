using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace API.Dtos
{
    public class RegisterDto
    {
         public string Id { get; set; }
        [Required]
        public int EmployeeId { get; set; } 
        public string EmployeeSAPCode { get; set; } 
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$", ErrorMessage= "Password must have 1 Uppercase, 1 Lowercase, 1 number, 1 non alphanumeric and at least 6 characters." )]
        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class SetRegisterDto
    {        
        public RegisterDto UserForm { get; set; }
        public RoleFormDto RoleForm { get; set; }
    }

    public class RoleFormDto
    {        
        public List<RoleDto> UserRoles { get; set; }
    }
}