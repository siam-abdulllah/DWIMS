using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class DoctorInfo : BaseEntity
    {
        public string DocotorCode { get; set; }
        public string DocotorName { get; set; }
        public string Degree { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}
