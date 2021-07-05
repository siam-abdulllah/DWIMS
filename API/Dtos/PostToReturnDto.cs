using System;
using System.Collections.Generic;

namespace API.Dtos
{
    public class PostToReturnDto
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string PostedBy { get; set; }
         public DateTimeOffset SetOn { get; set; }
        public IReadOnlyList<CommentToReturnDto> Comments { get; set; }
        
    }
}