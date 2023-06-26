using System.Net;
using CoreWCF.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace SessionNetTcpServer
{

    public class Program
    {

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //webBuilder.UseKestrel(options =>
                    //{
                    //    options.Listen(IPAddress.Any, 5000, listenOptions => { });
                    //    options.Listen(IPAddress.Any, 5001, listenOptions => { listenOptions.UseHttps(); });
                    //});
                    //webBuilder.UseNetTcp(IPAddress.Any, 5002);
                    webBuilder.UseStartup<Startup>();
                });

    }

}
