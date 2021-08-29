using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InstitutionInfo :BaseEntity
    {
      
        public int InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionType { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
        public int NoOfBeds { get; set; }
        public int AvgNoAdmtPati { get; set; }
    }
}
