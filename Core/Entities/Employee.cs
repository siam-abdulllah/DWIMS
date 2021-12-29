using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Employee : BaseEntity
    {
        public string EmployeeSAPCode { get; set; }
        public string EmployeeName { get; set; }
        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? DesignationId { get; set; }
        public string DesignationName { get; set; }
        public int? CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string JoiningPlace { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string DepotCode { get; set; }
        public string DepotName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; } 
        public string MarketGroupCode { get; set; }
        public string MarketGroupName { get; set; }
       
       
       
       
      

    }
}
