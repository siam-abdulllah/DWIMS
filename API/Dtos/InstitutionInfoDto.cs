using System;

namespace API.Dtos
{
    public class InstitutionInfoDto
    {
        public int Id { get; set; }
        public string InstitutionCode { get; set; }
        public string InstitutionName { get; set; }
        public string InstitutionType { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}