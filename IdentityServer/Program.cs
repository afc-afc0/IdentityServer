using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();
            
            using (var scope = builder.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                using (var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>())
                {
                    configurationDbContext.Clients.Add(new Client()
                    {
                        ClientId = "swagger",
                        AllowedCorsOrigins = new List<string>{"https://localhost:5003"}
                    }.ToEntity());
                    configurationDbContext.SaveChanges();
                    

                    // if (!configurationDbContext.Clients.Any())
                    // {
                    //     configurationDbContext.Clients.Add(new Client()
                    //     {
                    //         ClientId = "console",
                    //         ClientSecrets = new List<Secret>()
                    //         {
                    //             new Secret("secret".Sha256())
                    //         },
                    //         AllowedGrantTypes = GrantTypes.ClientCredentials,
                    //         AllowedScopes = new List<string>()
                    //         {
                    //             "api"
                    //         } 
                    //     }.ToEntity());
                    //     configurationDbContext.SaveChanges();
                    // }

                    // if (!configurationDbContext.ApiScopes.Any())
                    // {
                    //     configurationDbContext.ApiScopes.Add(new ApiScope("api").ToEntity());
                    //     configurationDbContext.SaveChanges();
                    // }

                    // if (!configurationDbContext.ApiResources.Any())
                    // {
                    //     configurationDbContext.ApiResources.Add(new ApiResource("api")
                    //     {
                    //         Scopes = new List<String>()
                    //         {
                    //             "api"
                    //         }
                    //     }.ToEntity());
                    //     configurationDbContext.SaveChanges();
                    // }
                }
            }

            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
