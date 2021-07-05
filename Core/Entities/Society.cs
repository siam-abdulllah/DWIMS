using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Entities
{
    public class Society : BaseEntity
    {
        public string SocietyName { get; set; }
        public string SocietyAddress { get; set; }
        public string NoOfMember { get; set; }
        public string Status { get; set; }
    }
}
