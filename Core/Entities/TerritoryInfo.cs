using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class TerritoryInfo : BaseEntity
    {
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }

    }
}
