using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace Orneholm.ApplicationInsights.HealthChecks.Sample
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
            services.AddApplicationInsightsTelemetry(Configuration);

            var random = new Random();

            services.AddHealthChecks()
                    .AddApplicationInsightsAggregatedAvailabilityPublisher()
                    .AddApplicationInsightsAvailabilityPublisher()
                    .AddSqlServer("FAKE", name: "SqlServer")
                    .AddAsyncCheck("Sample1", async () =>
                    {
                        await Task.Delay(random.Next(10, 200));
                        return HealthCheckResult.Healthy("Sample1Result", new Dictionary<string, object>
                        {
                            { "Key1", 1 },
                            { "Key2", "Sample" },
                        });
                    })
                    .AddAsyncCheck("Sample2", async () =>
                    {
                        await Task.Delay(random.Next(100, 500));
                        return HealthCheckResult.Degraded("Does not work 100%");
                    })
                    .AddAsyncCheck("Sample3", async () =>
                    {
                        await Task.Delay(random.Next(200, 800));
                        return HealthCheckResult.Unhealthy("Something is broken");
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.Run(async context =>
            {
                await context.Response.WriteAsync("Hello, World!");
            });
        }
    }
}
