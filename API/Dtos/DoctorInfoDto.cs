using System;

namespace API.Dtos
{
    public class DoctorInfoDto
    {
        public int Id { get; set; }
        public string DoctorCode { get; set; }
        public string DoctorName { get; set; }
        public string Degree { get; set; }
        public string Designation { get; set; }
        public string Address { get; set; }
        public string Status { get; set; }
    }
}