using System;

namespace API.Dtos
{
    public class CommentToReturnDto
    {
        public string CommentText { get; set; }
        public int NoOfLike { get; set; }
        public int NoOfDisLike { get; set; }   
        public string CommentBy  { get; set; }   
        public DateTimeOffset SetOn { get; set; }   

    }
}