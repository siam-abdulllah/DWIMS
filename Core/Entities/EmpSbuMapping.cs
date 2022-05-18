namespace Core.Entities
{
    public class EmpSbuMapping : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int EmployeeId { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public int Serial { get; set; }
        public string TagCode { get; set; }
    }
    public class EmpSbuMappingVM : EmpSbuMapping
    {
        public string EmployeeName { get; set; }
    }
}
