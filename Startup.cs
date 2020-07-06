using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace BookStore
{
    public class Startup
    {
        public  Startup(IConfiguration configuration)
	{
        Configuration=configuration;
	}
        private IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContextPool<AppDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("BooksDBConnection")));

            //services.AddMvc();

            services.AddIdentity<ApplicationUser, IdentityRole>
                (
                options => options.SignIn.RequireConfirmedEmail = true // to use confirmation email
                )
                    .AddEntityFrameworkStores<AppDBContext>()
                    .AddDefaultTokenProviders();


            services.AddControllersWithViews(
                config => 
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    config.Filters.Add(new AuthorizeFilter(policy)); 
                    //added a policy that restricts to view the pages without being logged in
                    //to get acces to different pages we put attribute [AllowAnonymous]
                }
                ).AddXmlSerializerFormatters();


            services.AddRazorPages();

            //Added Policy for Claim types
            services.AddAuthorization(options => options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete Role","true"))); // here added the second parameter, that checks if the value is set on true (case sensitive)


            // (*) Added Policy for Roles 
            //services.AddAuthorization(options => options.AddPolicy("AdminRolePolicy", policy => policy.RequireRole("Admin")));
            // (*)
            //services.AddSingleton<IBookStore, BookStoreRepository>();
            services.AddScoped<IBookStore, SQLBooksRepository>();

            //IdentityOptions to configure the user inputs (like password(add number of characters))
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization(); // for the Roles, we use authorization
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
