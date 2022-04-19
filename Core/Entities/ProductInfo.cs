namespace Core.Entities
{
    public class ProductInfo:BaseEntity
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public string BrandCode { get; set; }
        public string BrandName { get; set; }
        public string SBU { get; set; }
        public string SBUName { get; set; }
    }
}