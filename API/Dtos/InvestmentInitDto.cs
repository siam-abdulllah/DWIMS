using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class InvestmentInitDto
    {
        public int Id { get; set; }
        public string ReferenceNo { get; set; }
        public string ProposeFor { get; set; }

        public string DonationType { get; set; }
        public string DonationTo { get; set; }
        public int EmployeeId { get; set; }
        public EmployeeDto EmployeeDto { get; set; }

    }
}