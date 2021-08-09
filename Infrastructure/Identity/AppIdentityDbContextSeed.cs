using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
     public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    EmployeeId = 1,
                    Email = "bob@test.com",
                    UserName = "bob@test.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Bobbity",
                        Street = "10 The Street",
                        City = "New York",
                        State = "NY",
                        Zipcode = "90210"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
                
                // Can do anything
                 var role = new IdentityRole();
                 role.Id= "1";
                 role.Name = "Administrator";
                 role.NormalizedName = "Administrator";
                 await roleManager.CreateAsync(role);      

                // Only have Permission on Select & Insert & Edit & Delete in Specific Owner
                 role = new IdentityRole();
                 role.Id= "2";
                 role.Name = "Owner";
                 role.NormalizedName = "Owner";
                 await roleManager.CreateAsync(role);

                // Only have Permission on Select & Insert & Edit
                 role = new IdentityRole();
                 role.Id= "3";
                 role.Name = "Contributor";
                 role.NormalizedName = "Contributor";
                 await roleManager.CreateAsync(role);     

                 // Only have Permission on Select
                 role = new IdentityRole();
                 role.Id= "4";
                 role.Name = "Reader";
                 role.NormalizedName = "Reader";
                 await roleManager.CreateAsync(role);   

            }
        }
    }
}