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
}