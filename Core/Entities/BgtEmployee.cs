namespace Core.Entities
{
    public class BgtEmployee : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public int AuthId { get; set; }
        public int NoOfEmployee { get; set; }
        public long Amount { get; set; }
        //public string Segment { get; set; }
        public bool PermEdit { get; set; }
        public bool PermView { get; set; }
        public bool PermAmt { get; set; }
        public bool PermDonation { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
    }
    public class BgtEmployeeLocationWiseSBUExp
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public string SBU { get; set; }
        public int Year { get; set; }
        public long SBUAmount { get; set; }
        public int DonationId { get; set; }
        public string DonationTypeName { get; set; }
        public double Expense { get; set; }
        public int AuthId { get; set; }
        public string ApprovalAuthorityName { get; set; }
        public int TotalPerson { get; set; }
        public int TotalLoc { get; set; }
        public int Amount { get; set; }
        public int Limit { get; set; }
        public long? DonationTypeAllocated { get; set; }
        public long? TotalAllocated { get; set; }
    }
    public class BgtEmployeeVM:BgtEmployee
    {
        public string Authority { get; set; }
        public int Priority { get; set; }
        public int NoOfLoc { get; set; }
        public int BgtEmpId { get; set; }

    }
}
