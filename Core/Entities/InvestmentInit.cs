using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    class InvestmentInit : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }

        public string DonationType { get; set; }
        public string DonationTo { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }
        
    }
}
