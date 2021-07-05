using System.Collections.Generic;

namespace Core.Entities
{
    public class Post : BaseEntity
    {
        public string PostTitle { get; set; }
        public string PostDescription { get; set; }
        public string PostedBy  { get; set; }
        public ICollection<PostComments> PostComments { get; set; }
    }
}