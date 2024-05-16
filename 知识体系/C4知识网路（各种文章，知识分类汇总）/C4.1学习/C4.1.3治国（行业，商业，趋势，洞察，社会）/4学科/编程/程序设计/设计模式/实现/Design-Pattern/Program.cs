using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using System;

namespace NLogTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();
            services.AddLogging(loggingBuilder =>
            {
                // configure Logging with NLog
                loggingBuilder.ClearProviders();
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog("nlog.config");
            });

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);
            IConfiguration configuration = builder.Build();
            var serviceProvider = services.BuildServiceProvider();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            try
            {
                logger.LogInformation("Program Start");
                Console.WriteLine("Program Start");
                logger.LogInformation("Program End");
                Console.WriteLine("Program End");
                //uncommenting either of these lines makes the logging work
                //LogManager.Shutdown();
                //Thread.Sleep(3000);
            }
            catch
            {
                Console.WriteLine("Error");
            }
        }
    }
}
