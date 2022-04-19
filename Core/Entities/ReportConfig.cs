using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class ReportConfig: BaseEntity
    {
        public string ReportName { get; set; }
        public string ReportCode { get; set; }
        public string ReportFunc { get; set; }
    }
}
