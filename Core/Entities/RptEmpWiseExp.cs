using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class RptEmpWiseExp: BaseEntity
    {
        public int AuthId { get; set; }
        public string Budget { get; set; }
        public string EmployeeName { get; set; }
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int EmployeeId { get; set; }
        public int DonationId { get; set; }
        public string DonationTypeName { get; set; }
        public string SBU { get; set; }
        public string Segment { get; set; }
        public string Code { get; set; }
        public string CompoCode { get; set; }
        public int Year { get; set; }
        public long? TotalAmount { get; set; }
        public long? January { get; set; }
        public long? February { get; set; }
        public long? March { get; set; }
        public long? April { get; set; }
        public long? May { get; set; }
        public long? June { get; set; }
        public long? July { get; set; }
        public long? August { get; set; }
        public long? September { get; set; }
        public long? October { get; set; }
        public long? November { get; set; }
        public long? December { get; set; }
    }
}
