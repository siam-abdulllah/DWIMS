using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class RegionInfo : BaseEntity
    {
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

    }
}
