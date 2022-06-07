using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class CampaignExp
    {
        public int Id { get; set; }
        public string SBU { get; set; }
        public long? Amount { get; set; }
        public long? TotalAlloc { get; set; }
        public double? TotalExp { get; set; }
        public double? CampExp { get; set; }
    }
}
