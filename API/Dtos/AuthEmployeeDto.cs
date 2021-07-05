using System;

namespace API.Dtos
{
    public class AuthEmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
         public DateTimeOffset SetOn { get; set; }
         public DateTimeOffset ModifiedOn { get; set; }
    }
}
