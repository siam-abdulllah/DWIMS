namespace API.Dtos
{
    public class BgtEmpInsertDto
    {
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public int AuthId { get; set; }
        public long Amount { get; set; }
        public string Segment { get; set; }    
        public bool PermEdit { get; set; }
        public bool PermView { get; set; }
        public int EnteredBy { get; set; }
    }


    public class BgtOwnInsertDto
    {
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public int AuthId { get; set; }
        public long Amount { get; set; }
        public long AmtLimit { get; set; }
        public string Segment { get; set; }
        public int EnteredBy { get; set; }
        public int DonationId { get; set; }
    }
}



, @EnteredBy INT
, @DonationId INT