using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class ClusterDtl : BaseEntity
    {
        public int MstId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string Status { get; set; }
    }
}
