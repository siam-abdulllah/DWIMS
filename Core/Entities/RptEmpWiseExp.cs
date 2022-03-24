using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class RptEmpWiseExp: BaseEntity
    {
        public int ApprovalAuthorityId { get; set; }
        public int Priority { get; set; }
        public string EmployeeName { get; set; }
        public int EmployeeId { get; set; }
        public int DonationId { get; set; }
        public string DonationTypeName { get; set; }
        public string Month { get; set; }
        public int Year { get; set; }
        public double? Budget { get; set; }
        public double? Expense { get; set; }
    }
}
