using System;

namespace API.Dtos
{
    public class RegApprovalDto
    {
        public string UserId { get; set; }
        public int EmployeeId { get; set; }
        public string Role { get; set; }
        public string EmployeeSAPCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string MarketName { get; set; }
        public string RegionName { get; set; }
        public string ZoneName { get; set; }
        public string TerritoryName { get; set; }
        public string MarketGroupName { get; set; }
        public string SBU { get; set; }
        public string ApprovalStatus { get; set; }
    }
}
