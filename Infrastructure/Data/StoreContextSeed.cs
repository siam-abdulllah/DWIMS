using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Post.Any())
                {
                    var productsData = new Post
                    {
                        Id = 1,
                        PostTitle = "Post 1",
                        PostDescription = "NA",                       
                        PostedBy = "Admin",
                        DataStatus = 1
                    };

                    context.Add(productsData);
                    await context.SaveChangesAsync();

                    var productsData2 = new Post
                    {
                        Id = 2,
                        PostTitle = "Post 2",
                        PostDescription = "NA",                      
                        PostedBy = "Admin",
                        DataStatus = 1
                    };

                    context.Add(productsData2);
                    await context.SaveChangesAsync();

                    var productsData3 = new Post
                    {
                        Id = 3,
                        PostTitle = "Post 3",
                        PostDescription = "NA",                    
                        PostedBy = "Admin",
                        DataStatus = 1
                    };

                    context.Add(productsData3);
                    await context.SaveChangesAsync();
                }

                if (!context.PostComments.Any())
                {
                    // for Post 1
                    var comment = new PostComments{
                        Id=1,
                        PostId=1,
                        CommentText ="Comment 1",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 1"                        
                    };
                    context.Add(comment);
                    await context.SaveChangesAsync();

                    var comment2 = new PostComments{
                        Id=2,
                        PostId=1,
                        CommentText ="Comment 2",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 2"                        
                    };
                    context.Add(comment2);
                    await context.SaveChangesAsync();

                     var comment3 = new PostComments{
                        Id=3,
                        PostId=1,
                        CommentText ="Comment 3",
                        NoOfLike = 100,
                        NoOfDisLike =2,
                        CommentBy="User 3"                        
                    };
                    context.Add(comment3);
                    await context.SaveChangesAsync();

                    // Post 2
                     var comment11 = new PostComments{
                        Id=4,
                        PostId=2,
                        CommentText ="Comment 4",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 1"                        
                    };
                    context.Add(comment11);
                    await context.SaveChangesAsync();

                    var comment12 = new PostComments{
                        Id=5,
                        PostId=2,
                        CommentText ="Comment 5",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 2"                        
                    };
                    context.Add(comment12);
                    await context.SaveChangesAsync();


                    var comment13 = new PostComments{
                        Id=6,
                        PostId=2,
                        CommentText ="Comment 6",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 3"                        
                    };
                    context.Add(comment13);
                    await context.SaveChangesAsync();

                       var comment14 = new PostComments{
                        Id=7,
                        PostId=2,
                        CommentText ="Comment 7",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 3"                        
                    };
                    context.Add(comment14);
                    await context.SaveChangesAsync();

                    // post 3
                        var comment24 = new PostComments{
                        Id=8,
                        PostId=3,
                        CommentText ="Comment 8",
                        NoOfLike = 10,
                        NoOfDisLike =2,
                        CommentBy="User 1"                        
                    };
                    context.Add(comment24);
                    await context.SaveChangesAsync();

                }

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}