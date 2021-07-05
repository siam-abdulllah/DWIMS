using System;
using System.Text;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            var builder = services.AddIdentityCore<AppUser>();
            
            builder = new IdentityBuilder(builder.UserType, builder.Services);
            
            builder.AddRoles<IdentityRole>()
            .AddClaimsPrincipalFactory<UserClaimsPrincipalFactory<AppUser, IdentityRole>>()
            .AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.AddSignInManager<SignInManager<AppUser>>();

            services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequiredLength = 6;
                options.SignIn.RequireConfirmedEmail = true;
                
            });
          
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => 
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                    };
                });

            // services.AddAuthorization(options =>{
            //     options.AddPolicy("DetailUserPolicy",
            //         policy => policy.RequireClaim("CanViewDetails", "true"));
            // });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DetailUserPolicy", policy => 
                    policy.RequireRole("Owner"));
            });

            return services;
        }
    }    
}