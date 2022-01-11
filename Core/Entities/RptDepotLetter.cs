using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{    public class RptDepotLetter : BaseEntity
     {
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string DoctorName { get; set; }
        public long ProposedAmount { get; set; }
        public string Address { get; set; }
        public int DocId { get; set; }
        public string EmployeeName { get; set; }
        public int EmpId { get; set; }
        public string DesignationName { get; set; }
        public string MarketName { get; set; }
        public string DepotName { get; set; }
    }

    public class RptDepotLetterSearch : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string ProposeFor { get; set; }
        public string DoctorName { get; set; }
        public long ProposedAmount { get; set; }
        public string MarketName { get; set; }
        public string EmployeeName { get; set; }
    }
}
