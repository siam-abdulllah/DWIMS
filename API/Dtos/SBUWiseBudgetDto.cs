using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class SBUWiseBudgetDto
    {
        public int Id { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public DateTimeOffset? FromDate { get; set; }
        public DateTimeOffset? ToDate { get; set; }
        public long Amount { get; set; }
        public string Remarks { get; set; }
        public DateTimeOffset SetOn { get; set; }
    }
}
