using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class MarketGroupMst:BaseEntity
    {
        public string GroupName { get; set; }
        public string Status { get; set; }
        public string MarketCode { get; set; }
        //public int EmployeeId { get; set; }
        //[ForeignKey("EmployeeId")]
        //public Employee Employee { get; set; }
        [ForeignKey("MstId")]
        public virtual IEnumerable<MarketGroupDtl> MarketGroupDtls { get; set; }
    }
}
