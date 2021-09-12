using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
   public class ClusterMst : BaseEntity
    {
        public string ClusterCode { get; set; }
        public string ClusterName { get; set; }
        public string Status { get; set; }

        [ForeignKey("MstId")]
        public virtual IEnumerable<ClusterDtl> ClusterDtls { get; set; }
    }
}
