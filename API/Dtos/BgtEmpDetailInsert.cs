namespace API.Dtos
{
    public class BgtEmpDetailInsert
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public int AuthId { get; set; }
        public int EmployeeId { get; set; }
        public long Amount { get; set; }  
        public bool PermEdit { get; set; }
        public bool PermView { get; set; }
        public int EnteredBy { get; set; }
    }


   
}