using System;

namespace API.Dtos
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeSAPCode { get; set; }
        public string EmployeeName { get; set; }
        public int DepartmentId { get; set; }
        public int DesignationId { get; set; }
        public int CompanyId { get; set; }
        public DateTime JoiningDate { get; set; }
        public string JoiningPlace { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string PostingType { get; set; }
        public string MarketId { get; set; }
        public string MarketName { get; set; }
        public string RegionId { get; set; }
        public string RegionName { get; set; }
        public string ZoneId { get; set; }
        public string ZoneName { get; set; }
        public string TerritoryId { get; set; }
        public string TerritoryName { get; set; }
        public string DivisionId { get; set; }
        public string DivisionName { get; set; }
    }
}
