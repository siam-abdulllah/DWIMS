using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class EmpSbuMappingDto
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int EmployeeId { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }

    }
}
