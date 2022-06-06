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
        public int NoOfCamp { get; set; }
        public long Expense { get; set; }
        public long NewAmount { get; set; }
        public long Amount { get; set; }
        public long AllocatedBgt { get; set; }

    }
}