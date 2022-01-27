using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{

    public class MedicineDispatchDtl : BaseEntity
    {
        public int InvestmentInitId { get; set; }
        public InvestmentInit InvestmentInit { get; set; }
        public int ProductId { get; set; }
        public MedicineProduct MedicineProduct { get; set; }
        public int? EmployeeId { get; set; }
        public double TpVat { get; set; }
        public int BoxQuantity { get; set; }
        public int DispatchQuantity { get; set; }
        public double DispatchTpVat { get; set; }
    }
}
