using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class DocLocMap : BaseEntity
    {
        public int DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string SBU { get; set; }
    }
}
