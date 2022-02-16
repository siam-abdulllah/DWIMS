using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{    public class RptDepotLetter : BaseEntity
     {
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string DoctorName { get; set; }
        public string DonationTo { get; set; }
        public double ProposedAmount { get; set; }
        public string Address { get; set; }
        public int DocId { get; set; }
        public string EmployeeName { get; set; }
        public int EmpId { get; set; }
        public string DesignationName { get; set; }
        public string MarketName { get; set; }
        public string DepotName { get; set; }
        public string ChequeTitle { get; set; }
        public string ApprovedBy { get; set; }
    }

    public class RptDepotLetterSearch : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public string ReferenceNo { get; set; }
        public string PayRefNo { get; set; }
        public string DonationTypeName { get; set; }
        public string DonationTo { get; set; }
        public string ProposeFor { get; set; }
        public int DId { get; set; }
        public string DoctorName { get; set; }
        public double ProposedAmount { get; set; }
        public string MarketName { get; set; }
        public string EmployeeName { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovedDate { get; set; }
    }

    public class MedDispSearch : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public string PayRefNo { get; set; }
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string ProposeFor { get; set; }
        public string DoctorName { get; set; }
        public double ProposedAmount { get; set; }
        public string MarketName { get; set; }
        public string EmployeeName { get; set; }
        public string ApprovedBy { get; set; }
        public string DepotName { get; set; }
        public DateTimeOffset ApprovedDate { get; set; }
    }


    public class RptMedDisp : BaseEntity
    {
        public string ReferenceNo { get; set; }
        public string DonationTypeName { get; set; }
        public string EmployeeName { get; set; }
        public string ApprovedBy { get; set; }
        public DateTimeOffset ApprovedDate { get; set; }
        public string MarketName { get; set; }
        public string PaymentRefNo { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public double? DispatchAmt { get; set; }
        public string Remarks { get; set; }
    }
}
