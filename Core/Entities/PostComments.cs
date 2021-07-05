
namespace Core.Entities
{
    public class PostComments : BaseEntity
    {
       
        public string CommentText { get; set; }
        public int NoOfLike { get; set; }
        public int NoOfDisLike { get; set; }   
        public string CommentBy  { get; set; }
        public int PostId { get; set; }            
        public Post Post { get; set; }
    }
}