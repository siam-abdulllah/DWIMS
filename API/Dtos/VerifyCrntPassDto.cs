using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class VerifyCrntPassDto
    {
        public string EmployeeSAPCode { get; set; }
        public string CurrentPassword { get; set; }
    }
}
