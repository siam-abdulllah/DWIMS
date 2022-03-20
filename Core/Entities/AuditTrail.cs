using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class AuditTrail : BaseEntity
    {
        public string ActionName { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionDate { get; set; }
        public string ActionTable { get; set; }
        public string FormName { get; set; }
        public string ActionBy { get; set; }
        public string HostAddress { get; set; }
        public string TransID { get; set; }
    }
}
