using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspNetCoreToDo
{
    public class Program
    {
        public static void Main(string[] args){
            var host = CreateHostBuilder(args).Build();
            InitializeDatabase(host);
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            Action<IWebHostBuilder> configure = webBuilder => webBuilder.UseStartup<Startup>();
            return Host.CreateDefaultBuilder(args)
                            .ConfigureWebHostDefaults(configure);
        }

        private static void InitializeDatabase(IHost host){
            using (var scope  = host.Services.CreateScope()){
                var services = scope.ServiceProvider;

                try{
                    SeedData.InitializeAsync(services).Wait();
                }catch(Exception ex){
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Error occured while seeding the database!");
                }
            }
        }
    }
}
