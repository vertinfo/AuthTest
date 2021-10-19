using AuthTest.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthTest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                builder.AddUserSecrets<Program>();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            IConfigurationRoot configuration = builder.Build();

            SetupStaticLogger(configuration);

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }

        private static void SetupStaticLogger(IConfigurationRoot configuration)
        {
            //var sc = CloudStorageAccount.Parse(configuration["LogBlobConn"]);

            var loggers = new LoggerConfiguration()
                  .MinimumLevel.Information()
                  .Enrich.FromLogContext();

            loggers.WriteTo.Logger(logger => logger
                    .WriteTo.Console()
                    .WriteTo.AzureBlobStorage(connectionString: configuration["LogBlobConn"], restrictedToMinimumLevel: LogEventLevel.Information, storageContainerName: "qitestlogs", "authtest/{yyyy}/{MM}/{dd}/{yyyy}-{MM}-{dd}_{HH}.txt")
                    .WriteTo.AzureApp());
            Log.Logger = loggers.CreateLogger();
        }

    }
}
