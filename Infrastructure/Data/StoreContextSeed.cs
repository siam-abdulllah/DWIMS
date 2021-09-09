using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Employee.Any())
                {
                   
                    var empsData = new Employee
                    {
                        //Id = 1,
                        EmployeeName = "Test",
                        EmployeeSAPCode = "1",
                        DepartmentId = 1,
                        DepartmentName = "Sales",
                        DesignationId =1,
                        DesignationName = "MPO",
                        CompanyId = 1,
                        CompanyName = "Square Pharmaceuticals Ltd.",
                        JoiningDate = DateTime.Now,
                        Phone = "0171111111",
                        Email = "test@gmail.com",
                        MarketGroupCode = "All",
                        MarketGroupName = "Barisal-1(A)",
                        MarketCode = "F24",
                        MarketName = "Barisal-1(A)",
                        RegionCode = "A41",
                        RegionName = "Barisal(A)",
                        ZoneCode = "Z",
                        ZoneName = "Zone-All",
                        TerritoryCode = "A00007",
                        TerritoryName = "Barishal-A(A)",
                        SBU = "1",
                        SBUName = "A",
                        DataStatus = 1
                    };
                    context.Add(empsData);
                    await context.SaveChangesAsync();
                }

                //if (!context.PostComments.Any())
                //{
                //    // for Post 1
                //    var comment = new PostComments{
                //        Id=1,
                //        PostId=1,
                //        CommentText ="Comment 1",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 1"                        
                //    };
                //    context.Add(comment);
                //    await context.SaveChangesAsync();

                //    var comment2 = new PostComments{
                //        Id=2,
                //        PostId=1,
                //        CommentText ="Comment 2",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 2"                        
                //    };
                //    context.Add(comment2);
                //    await context.SaveChangesAsync();

                //     var comment3 = new PostComments{
                //        Id=3,
                //        PostId=1,
                //        CommentText ="Comment 3",
                //        NoOfLike = 100,
                //        NoOfDisLike =2,
                //        CommentBy="User 3"                        
                //    };
                //    context.Add(comment3);
                //    await context.SaveChangesAsync();

                //    // Post 2
                //     var comment11 = new PostComments{
                //        Id=4,
                //        PostId=2,
                //        CommentText ="Comment 4",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 1"                        
                //    };
                //    context.Add(comment11);
                //    await context.SaveChangesAsync();

                //    var comment12 = new PostComments{
                //        Id=5,
                //        PostId=2,
                //        CommentText ="Comment 5",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 2"                        
                //    };
                //    context.Add(comment12);
                //    await context.SaveChangesAsync();


                //    var comment13 = new PostComments{
                //        Id=6,
                //        PostId=2,
                //        CommentText ="Comment 6",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 3"                        
                //    };
                //    context.Add(comment13);
                //    await context.SaveChangesAsync();

                //       var comment14 = new PostComments{
                //        Id=7,
                //        PostId=2,
                //        CommentText ="Comment 7",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 3"                        
                //    };
                //    context.Add(comment14);
                //    await context.SaveChangesAsync();

                //    // post 3
                //        var comment24 = new PostComments{
                //        Id=8,
                //        PostId=3,
                //        CommentText ="Comment 8",
                //        NoOfLike = 10,
                //        NoOfDisLike =2,
                //        CommentBy="User 1"                        
                //    };
                //    context.Add(comment24);
                //    await context.SaveChangesAsync();

                //}

            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}