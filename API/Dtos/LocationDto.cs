using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class LocationDto
    {
    }

    public class TerrirotyDto
    {
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
    }
    public class MarketLocDto
    {
        public string MarketCode { get; set; }
        public string MarketName { get; set; }
    }
    public class RegionDto
    {
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
    }
    public class DivisionDto
    {
        public string DivisionCode { get; set; }
        public string DivisionName { get; set; }
    }
    public class ZoneDto
    {
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
    }
}
