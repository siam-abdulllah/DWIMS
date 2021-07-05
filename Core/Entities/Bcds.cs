using System.Collections.Generic;

namespace Core.Entities
{
    public class Bcds : BaseEntity
    {
        public string BcdsName { get; set; }
        public string BcdsAddress { get; set; }
        public string NoOfMember  { get; set; }
        public string Status { get; set; }
    }
}