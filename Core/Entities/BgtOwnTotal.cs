namespace Core.Entities
{
    public class BgtOwnTotal : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int AuthId { get; set; }
        public string Code { get; set; }
        public string SBU { get; set; }
        public string CompoCode { get; set; }
        public int DonationId { get; set; }
        public long Amount { get; set; }
        public long AmtLimit { get; set; }
        public string Segment { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
        public long TotalAmount { get; set; }  
        
    }


   
}