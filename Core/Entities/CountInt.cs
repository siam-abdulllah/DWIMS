using Microsoft.EntityFrameworkCore;

namespace Core.Entities
{

    public class CountInt : BaseEntity
    {  
        public int Count { get; set; }
    }

    public class CountDouble : BaseEntity
    {
        public long Count { get; set; }
    }
}