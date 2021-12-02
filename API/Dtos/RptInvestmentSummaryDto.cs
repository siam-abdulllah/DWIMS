using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class RptInvestmentSummaryDto
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string DonationTo { get; set; }
        public int ProposedAmount { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string InvStatus { get; set; }
        public string EmployeeName { get; set; }
    }
}
