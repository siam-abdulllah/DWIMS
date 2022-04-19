using Core.Entities;

namespace API.Dtos
{
    public class MedicineDispatchDtlDto
    {
        public int Id { get; set; }
        public int InvestmentInitId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? EmployeeId { get; set; }
        public double TpVat { get; set; }
        public int BoxQuantity { get; set; }
        public int DispatchQuantity { get; set; }
        public double DispatchTpVat { get; set; }
    }
}
