using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class BgtYearlyTotalDto
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public long TotalAmount { get; set; }
        public long NewAmount { get; set; }
        public long AddAmount { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
        public DateTimeOffset SetOn { get; set; }
        public int DataStatus { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
    }
}
