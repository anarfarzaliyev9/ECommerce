using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMVC_ECommerceApp.Abstractions;
using AspNetCoreMVC_ECommerceApp.Contexts;
using AspNetCoreMVC_ECommerceApp.Profiles;
using AspNetCoreMVC_ECommerceApp.Repositories;
using AutoMapper;
using ECommerce_API.Abstractions;
using ECommerce_API.Models;
using ECommerce_Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AspNetCoreMVC_ECommerceApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(option => option.EnableEndpointRouting = false);
            //For dependency injection
            services.AddScoped(typeof(IProductRepo), typeof(ProductRepo));
            services.AddScoped(typeof(ICategoryRepo), typeof(CategoryRepo));
            services.AddScoped(typeof(ICartRepo), typeof(CartRepo));
            services.AddScoped(typeof(ICartProductRepo), typeof(CartProductRepo));
            //Add database
            services.AddDbContext<AppDbContext>(options =>
                       options.UseSqlServer(Configuration.GetConnectionString("DBConnection")));
            //Add identity to same database
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
             {
                 options.Password.RequiredUniqueChars = 0;
             }).AddEntityFrameworkStores<AppDbContext>();
            // Configure mapper
            var mapperConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ProductProfile());
            });
            IMapper mapper = mapperConfig.CreateMapper();
            services.AddSingleton(mapper);
            
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Admin}/{action=Index}/{id?}"
                );
            });
        }
    }
}
