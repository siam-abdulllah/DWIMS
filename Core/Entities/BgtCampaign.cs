using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class BgtCampaign : BaseEntity
    {
        public int CompId { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public string SBU { get; set; }
        public long Amount { get; set; }
        public string Remarks { get; set; }
        public int EnteredBy { get; set; }
    }
}
