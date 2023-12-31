﻿using CoreWCF;
using CoreWCF.Configuration;
using CoreWCF.Description;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SessionNetTcpServer.Services;

namespace SessionNetTcpServer
{

    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddTransient<CalculatorService>();
            services.AddScoped<SpecialService>();
            services.AddSingleton<CallbackService>();

            services
                .AddServiceModelServices()  //Enable CoreWCF Services
                .AddServiceModelMetadata(); //Enable metadata (WSDL) support

            services
                .AddSingleton<IServiceBehavior, UseRequestHeadersForMetadataAddressBehavior>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            string html = @"
            <!DOCTYPE html>
            <html>
                <title>SessionNetTcpServer</title>
                <body>
                    <div>You found the server</div>
                </body>
            </html>
            ";

            app.UseEndpoints
            (
                
                endpoints =>
                {
                    endpoints.MapGet("/", async context => { await context.Response.WriteAsync(html); });
                }
            );

            app.UseServiceModel(builder =>
            {

                // add the calculator service with BasicHttpBinding
                builder
                    .AddService<CalculatorService>(serviceOptions => { })
                    .AddServiceEndpoint<CalculatorService, ICalculatorService>(new BasicHttpBinding(), "/Calculator/basicHttp");

                //// add the special service with NetTcpBinding
                //builder
                //    .AddService<SpecialService>(serviceOptions => { })
                //    .AddServiceEndpoint<SpecialService, ISpecialService>(new NetTcpBinding(SecurityMode.None), "/Special/netTcp");

                //// add the callback service with NetTcpBinding
                //builder
                //    .AddService<CallbackService>(serviceOptions => { })
                //    .AddServiceEndpoint<CallbackService, ICallbackService>(new NetTcpBinding(SecurityMode.None), "/Callback/netTcp");


                // Configure WSDL to be available
                var serviceMetadataBehavior = app.ApplicationServices.GetRequiredService<CoreWCF.Description.ServiceMetadataBehavior>();
                serviceMetadataBehavior.HttpGetEnabled = true;
                serviceMetadataBehavior.HttpsGetEnabled = true;
            });

        }

    }

}
