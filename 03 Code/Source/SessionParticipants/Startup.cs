using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bluehands.Diagnostics.LogExtensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using SessionParticipants.Domain;

namespace SessionParticipants
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddCors(o => o.AddPolicy("AllowAll", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));
            var configuredMeetings = new ConfiguredMeetings(Configuration);
            services.AddSingleton(configuredMeetings);
            services.Configure<ZoomCredentials>(Configuration.GetSection("ZoomCredentials"));
            services.Configure<GeneralSettings>(Configuration.GetSection("GeneralSettings"));
            services.AddLogging(loggingBuilder => { loggingBuilder.AddLogEnhancementWithNLog(); });
            LogManager.Configuration = new NLogLoggingConfiguration(Configuration.GetSection("NLog"));
            var useTestData = Configuration.GetSection("GeneralSettings:UseTestData").Value;
            services.AddSingleton<ISessionRepository, SessionRepository>();
            if (!string.IsNullOrEmpty(useTestData))
            {
                if (bool.TryParse(useTestData,out bool b)){
                    if (b)
                    {
                        services.AddSingleton<ISessionRepository, SessionRepositoryMock>();
                    }
                }
            }
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            var log = new Log<Startup>();
            app.UseLogCorrelation(log);
            app.UseRequestLogTracing(log);
            if (!env.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();
            app.UseCors("AllowAll");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
