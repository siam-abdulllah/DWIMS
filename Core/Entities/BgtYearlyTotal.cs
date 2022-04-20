namespace Core.Entities
{
    public class BgtYearlyTotal : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public long TotalAmount { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
    }
}
