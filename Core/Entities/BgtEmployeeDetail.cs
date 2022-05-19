namespace Core.Entities
{
    public class BgtEmployeeDetail : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public int AuthId { get; set; }
        public string Code { get; set; }
        public string CompoCode { get; set; }
        public long Amount { get; set; }
        public bool PermEdit { get; set; }
        public bool PermView { get; set; }
        public bool PermAmt { get; set; }
        public bool PermDonation { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
    }


   
}
