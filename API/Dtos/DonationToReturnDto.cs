using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class DonationToReturnDto
    {
        public int Id { get; set; }
        public string DonationTypeName { get; set; }
        public string DonationShortName { get; set; }
        public string Remarks { get; set; }
         public string Status { get; set; }
         public DateTimeOffset SetOn { get; set; }
        
    }
}