using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class BgtSbuYearlyTotalDto
    {
        public int Id { get; set; }
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public long SBUAmount { get; set; }
        public string SBUName { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
        public List<SbuDetails> SbuDetailsList { get; set; }
    }
    public class SbuDetails
    {
        public string SBUName { get; set; }
        public string SBUCode { get; set; }
        public long SBUAmount { get; set; }
        public long NewAmount { get; set; }
    }
}
