using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{

    public class CountInt : BaseEntity
    {  
        public int Count { get; set; }
    }

    public class CountLong : BaseEntity
    {
        public long Count { get; set; }
    }

    public class CountDouble : BaseEntity
    {
        public double Count { get; set; }
    }
    public class DonWiseExpByEmp : BaseEntity
    {
        public int DonationId { get; set; }
        public double Count { get; set; }
    }
}