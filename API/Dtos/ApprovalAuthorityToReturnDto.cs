using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class ApprovalAuthorityToReturnDto
    {
        public int Id { get; set; }
        public string ApprovalAuthorityName { get; set; }
        public string Remarks { get; set; }
         public string Status { get; set; }
         public int Priority { get; set; }

         public DateTimeOffset SetOn { get; set; }
         public DateTimeOffset ModifiedOn { get; set; }
        
    }
}