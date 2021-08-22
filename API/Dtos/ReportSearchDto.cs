using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ReportSearchDto
    {
        public int UserId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string SBU { get; set; }
        public string Location { get; set; }
        public string DonationType { get; set; }
        public string Name { get; set; }
    }
}
