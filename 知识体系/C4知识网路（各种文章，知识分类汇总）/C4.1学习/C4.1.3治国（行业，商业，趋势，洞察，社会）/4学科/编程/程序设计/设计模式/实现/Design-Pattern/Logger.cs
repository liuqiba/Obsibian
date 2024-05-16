// using Microsoft.Extensions.Configuration;
// using Microsoft.Extensions.DependencyInjection;
// using Microsoft.Extensions.Logging;
// using NLog;
// using NLog.Extensions.Logging;
// 
// namespace Design_Pattern
// {
//     class Logger
//     {
// 
//         public static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
//         
// 
//         public void Test()
//         {
// 
// 
//             logger.Debug("sadf");
//             var config = new ConfigurationBuilder()
//          .SetBasePath(System.IO.Directory.GetCurrentDirectory()) //From NuGet Package Microsoft.Extensions.Configuration.Json
//          .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//          .Build();
//         }
//     }
// 
// 
// }

