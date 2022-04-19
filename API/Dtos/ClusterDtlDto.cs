using System;

namespace API.Dtos
{
    public class ClusterDtlDto
    {
        public int Id { get; set; }
        public int MstId { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }
        public string Status { get; set; }

    }
}