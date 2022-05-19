using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class BgtEmployeeDto
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public int AuthId { get; set; }
        public int NoOfEmployee { get; set; }
        public long Amount { get; set; }
        public long NewAmount { get; set; }
        //public string Segment { get; set; }
        public bool PermEdit { get; set; }
        public bool PermView { get; set; }
        public bool PermAmt { get; set; }
        public bool PermDonation { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
    }

    public class BgtEmployeeModel
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public string SBUCode { get; set; }
        public List<BgtEmployeeDto> bgtEmpList { get; set; }
    }
}
