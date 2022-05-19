namespace API.Dtos
{
    public class BgtOwnDetailInsert
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public string Segment { get; set; }
        public int AuthId { get; set; }
        public int EmployeeId { get; set; }
        public int DonationId { get; set; }
        public long Amount { get; set; }  
        public long AmtLimit { get; set; }  
        public int EnteredBy { get; set; }
    }


   
}