using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class AppAuthDetailsDto
    {
        public int Id { get; set; }
        public int DataStatus { get; set; }
        public DateTimeOffset SetOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        public string Authority { get; set; }
        public int Priority { get; set; }
        public int AuthId { get; set; }
        public int NoOfEmployee { get; set; }
        public int Expense { get; set; }
        public int NewAmount { get; set; }
        public int NoOfLoc { get; set; }
        public int Amount { get; set; }

    }
}