using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ReportConfigDto
    {
        public int Id { get; set; }
        public string ReportName { get; set; }
        public string ReportCode { get; set; }
        public string ReportFunc { get; set; }
    }
}
