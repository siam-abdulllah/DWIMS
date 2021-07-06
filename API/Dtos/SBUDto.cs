using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class SBUDto
    {
        public int Id { get; set; }
        public string SBUName { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset SetOn { get; set; }
    }
}
