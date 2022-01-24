using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using System;

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

            services.AddIdentityServer()
                .AddConfigurationStore(options => 
                {
                    options.ConfigureDbContext = ConfigureDbContext;
                })
                .AddOperationalStore(options => 
                {
                    options.ConfigureDbContext = ConfigureDbContext;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
        }

        
    }
}
