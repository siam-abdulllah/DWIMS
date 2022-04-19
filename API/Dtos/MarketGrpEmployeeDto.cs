using System;

namespace API.Dtos
{
    public class MarketGrpEmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public DateTimeOffset SetOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string SBU { get; set; }

    }
}
