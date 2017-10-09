using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Enfile.Application
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if(!IsInitDb(args))
            {
                BuildWebHost().Run();   
            }else
            {
                StartupDB.Create();
            }
        }

        public static IWebHost BuildWebHost() => new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>().Build();

        public static bool IsInitDb(string[] args) => (args.Length > 0 && args[0] == "--initdb");
    }
}
