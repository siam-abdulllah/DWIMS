namespace Core.Entities
{
    public class EmpSbuMapping : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int EmployeeId { get; set; }
        public string SBU { get; set; }
    }
}
