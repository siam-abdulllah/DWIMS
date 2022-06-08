using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class CampaignBgtDetailsDto
    {
        public int Id { get; set; }
        public int DataStatus { get; set; }
        public DateTimeOffset SetOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string SBU { get; set; }
        public int DeptId { get; set; }
        public int Year { get; set; }
        public int NoOfCamp { get; set; }
        public double Expense { get; set; }
        public double NewAmount { get; set; }
        public double Amount { get; set; }
        public double AllocatedBgt { get; set; }

    }
}