namespace Core.Entities
{
    public class MedicineProduct : BaseEntity
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string PackSize { get; set; }
        public string Status { get; set; }
        public double UnitTp { get; set; }
        public double UnitVat { get; set; }
        public string SorgaCode { get; set; }
       
    }
}