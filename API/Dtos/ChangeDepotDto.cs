using Core.Entities;
using System;

namespace API.Dtos
{
    public class ChangeDepotDto
    {   
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public DateTimeOffset ChangeDate { get; set; }
        public int? EmployeeId { get; set; }
        public Employee Employee { get; set; }
        public string Remarks { get; set; }
        public string OldDepotCode { get; set; }
        public string DepotCode { get; set; }
    }
}
