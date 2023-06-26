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
                    //webBuilder.UseNetTcp(8808);
                    webBuilder.UseStartup<Startup>();
                });

    }

}
