using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System;
using IdentityServer.Data;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
     
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            void ConfigureDbContext(DbContextOptionsBuilder builder)
            {
                builder.UseNpgsql(Configuration.GetConnectionString("DefaultConnection"), optionsBuilder =>
                {
                    optionsBuilder.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                });
            }

            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(ConfigureDbContext);

            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddIdentityServer()
                .AddConfigurationStore(options => { options.ConfigureDbContext = ConfigureDbContext; })
                .AddOperationalStore(options => { options.ConfigureDbContext = ConfigureDbContext; })
                .AddAspNetIdentity<IdentityUser>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseIdentityServer();

            app.UseIdentityServer();

            app.UseAuthorization();

            app.UseEndpoints(builder => 
            {
                builder.MapDefaultControllerRoute();
            });
        }

        
    }
}
