
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities

{
    public class ApprovalAuthority:BaseEntity
    {
        public string ApprovalAuthorityName { get; set; }
        public string Remarks { get; set; }
        public string Status { get; set; }
        public int Priority { get; set; }

    }
}