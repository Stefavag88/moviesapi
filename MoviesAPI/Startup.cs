using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoviesAPI.Extensions;
using MoviesAPI.Middleware;
using Newtonsoft.Json.Serialization;
using NLog;
using Swashbuckle.AspNetCore.Swagger;

namespace MoviesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureLoggerService();
            services.ConfigureMoviesService();
            services.ConfigureAdminService();
            services.AddDbContext<MoviesDBContext>(options =>
            {
                var connectionString = Configuration.GetSection("MoviesDbConnection:connectionString").Value;

                options
                    .UseSqlServer(connectionString);
            });
            services.AddSwaggerGen(swagger =>
            {
                swagger.EnableAnnotations();
                swagger.DescribeAllEnumsAsStrings();
                swagger.DescribeAllParametersInCamelCase();
                swagger.SwaggerDoc("v1", new Info { Title = "Swagger Movies API" });

                var swaggerCommentsXml = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var swaggerXmlPath = Path.Combine(AppContext.BaseDirectory, swaggerCommentsXml);

                swagger.IncludeXmlComments(swaggerXmlPath);
            });
            services.AddMvc()
                    .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                    .AddJsonOptions(options => {
                        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger Movies API");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute("admin", "{controller}/{action=Index}/{id?}", defaults: new { controller = "Admin" });
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");

            });

        }
    }
}
