using System;

namespace API.Dtos
{
    public class BcdsDto
    {
        public int Id { get; set; }
        public string BcdsName { get; set; }
        public string BcdsAddress { get; set; }
        public string NoOfMember { get; set; }
        public string Status { get; set; }
        public DateTimeOffset SetOn { get; set; }
    }
}