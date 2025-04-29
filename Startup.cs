using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Demo03.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Demo03.Services;
using Microsoft.AspNetCore.SignalR;
using Demo03.Hubs;
using Demo03.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Demo03
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));
            services.AddDefaultIdentity<IdentityUser>(options => 
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
            services.AddControllersWithViews();
            services.AddRazorPages();
            
            // Simplified authorization policies
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                
                options.AddPolicy("RequireManagerRole", policy => 
                    policy.RequireRole("Manager"));
                
                options.AddPolicy("RequireTeacherRole", policy => 
                    policy.RequireRole("Teacher"));
                
                options.AddPolicy("RequireStudentRole", policy => 
                    policy.RequireRole("Student"));
            });
            
            // Add SignalR
            services.AddSignalR();
            
            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddScoped<IEmailService, EmailService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
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
            
            // Use CORS
            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                
                // Add SignalR hub endpoint
                endpoints.MapHub<ChatHub>("/chatHub");
            });

            // Create roles
            CreateRoles(serviceProvider).Wait();
        }
        
        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // Create Manager role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Manager"))
            {
                await roleManager.CreateAsync(new IdentityRole("Manager"));
            }

            // Create Teacher role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Teacher"))
            {
                await roleManager.CreateAsync(new IdentityRole("Teacher"));
            }

            // Create Student role if it doesn't exist
            if (!await roleManager.RoleExistsAsync("Student"))
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }
        }
    }
}