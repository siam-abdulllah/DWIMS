using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class SubCampaignToReturnDto
    {
        public int Id { get; set; }
        public string SubCampaignName { get; set; }
        public string Remarks { get; set; }
         public string Status { get; set; }
         public DateTimeOffset SetOn { get; set; }
        
    }
}