using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{

    public class MedicineDispatchDtl : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public int ProductId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string SBU { get; set; }
        public int? EmployeeId { get; set; }
        public double TpVat { get; set; }
        public int BoxQuantity { get; set; }
        public int DispatchQuantity { get; set; }
        public double DispatchTpVat { get; set; }
    }
}
