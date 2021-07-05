using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class EmployeePostingDto
    {
        public int Id { get; set; }
        public string EmployeeId { get; set; }
        public DateTime PostingDate { get; set; }
        public string ZoneId { get; set; }
        public string DivisionId { get; set; }
        public string RegionId { get; set; }
        public string TerritoryId { get; set; }
        public string MarketId { get; set; }
    }
}
