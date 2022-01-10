namespace Core.Entities
{
    public class EmployeeLocation : BaseEntity
    {
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string EmployeeName { get; set; }
        public string Priority { get; set; }
    }
}
