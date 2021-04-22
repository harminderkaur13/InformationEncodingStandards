using IdentityExample.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityExample
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(config =>
            {
                config.UseInMemoryDatabase("Memory");
            });

            
            // AddIdentity registers the services
            services.AddIdentity<IdentityUser, IdentityRole>(config =>
                
                {
                    config.Password.RequiredLength = 4;
                    config.Password.RequireDigit = false;
                    config.Password.RequireNonAlphanumeric = false;
                    config.Password.RequireUppercase = false;
                   // config.SignIn.RequireConfirmedEmail = true;
                })

                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "security.Cookie";
                config.LoginPath = "/Home/Login";
            });




            // services.AddMailKit(config => config.UseMailKit(_config.GetSection("Email").Get<MailKitOptions>()));
            // services.AddAuthentication("CookieAuth")
            //   .AddCookie("CookieAuth", config =>
            //  {
            //    config.Cookie.Name = "Web.Development";
            //     config.LoginPath = "/Home/Authenticate";
            //  });
            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            //who are you?
            app.UseAuthentication();
            //are you allowed?
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
