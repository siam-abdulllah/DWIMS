using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Core.Entities
{
    public class InvestmentRapidAppr : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public int InvestmentRapidId { get; set; }
        public int ApprovedBy{get;set;}
        public string ApprovalRemarks { get; set; }
        public string ApprovedStatus{get;set;}
        public int ApproverId { get; set; }
    }
}
