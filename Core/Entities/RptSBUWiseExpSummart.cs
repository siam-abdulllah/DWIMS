namespace Core.Entities
{
    public class RptSBUWiseExpSummart : BaseEntity
    {
        public string SBUName { get; set; }
        public string SBU { get; set; }
        public string DonationTypeName { get; set; }
        public int DonationId { get; set; }
        public long Expense { get; set; }
        public long Budget { get; set; }
    }

    public class RptEmpWiseExpSummary : BaseEntity
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Duration { get; set; }
        public string DonationTypeName { get; set; }
        public int DonationId { get; set; }
        public long Expense { get; set; }
        public long Budget { get; set; }
    }
}
