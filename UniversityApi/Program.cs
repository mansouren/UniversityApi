using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace UniversityApi
{
    public class Program
    {
        //    public static void Main(string[] args)
        //    {  
        //        //Set deafult proxy
        //        WebRequest.DefaultWebProxy = new WebProxy("http://127.0.0.1:8118", true) { UseDefaultCredentials = true };

        //        var logger = LogManager.GetCurrentClassLogger();

        //        try
        //        {
        //            logger.Debug("init main");
        //            CreateHostBuilder(args).Build().RunAsync();
        //        }
        //        catch (Exception exception)
        //        {
        //            //NLog: catch setup errors
        //            logger.Error(exception, "Stopped program because of exception");
        //            throw;
        //        }
        //        finally
        //        {
        //            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
        //            LogManager.Flush();
        //            LogManager.Shutdown();
        //        }
        //    }

        //    public static IHostBuilder CreateHostBuilder(string[] args) =>
        //        Host.CreateDefaultBuilder(args)
        //            .ConfigureWebHostDefaults(webBuilder =>
        //            {

        //                webBuilder.UseStartup<Startup>();
        //            })
        //            .ConfigureLogging(logging =>
        //            {
        //                logging.ClearProviders();
        //                //logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //            })
        //            .UseNLog();  // NLog: Setup NLog for Dependency injection;
        //}
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
