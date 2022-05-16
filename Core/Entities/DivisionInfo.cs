using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class DivisionInfo : BaseEntity
    {
        public string SBU { get; set; }
        public string SBUName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }

    }
}
