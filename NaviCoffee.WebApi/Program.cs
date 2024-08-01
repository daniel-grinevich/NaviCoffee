using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace NaviCoffee.WebApi
{
    public class Program
    {
        /*
            Main code. This is the entry point of any C# application
        */
        public static void Main(string[] args)
        {
            /*
                This method sets up and configures the host weith pre-configured defaults which include:
                Web server (kestrel) and logging and configuration sources like appsettings.json
                Dependcy injection container setup
            */
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    /*
                        This method configures the settings for the web host that will host your application:
                    */
                    webBuilder.UseStartup<Startup>();
                });
    }
}
