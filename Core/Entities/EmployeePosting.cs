using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class EmployeePosting : BaseEntity
    {
        public string EmployeeId { get; set; }
        public DateTime PostingDate { get; set; }
        public string ZoneId { get; set; }
        public string DivisionId { get; set; }
        public string RegionId { get; set; }
        public string TerritoryId { get; set; }
        public string MarketId { get; set; }

    }
}
