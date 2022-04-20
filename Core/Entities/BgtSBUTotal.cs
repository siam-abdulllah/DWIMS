namespace Core.Entities
{
    public class BgtSBUTotal : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public long SBUAmount { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
    }
}
