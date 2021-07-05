using System;

namespace API.Dtos
{
    public class SocietyDto
    {
        public int Id { get; set; }
        public string SocietyName { get; set; }
        public string SocietyAddress { get; set; }
        public string NoOfMember { get; set; }
        public string Status { get; set; }
        public DateTimeOffset SetOn { get; set; }
    }
}
