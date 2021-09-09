using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class EmployeePosting : BaseEntity
    {
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public DateTimeOffset PostingDate { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
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
