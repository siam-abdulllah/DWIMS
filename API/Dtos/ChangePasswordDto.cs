using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ChangePasswordDto
    {
        public string EmployeeSAPCode { get; set; }
        public string NewPassword { get; set; }
        public string CurrentPassword { get; set; }
    }
}
