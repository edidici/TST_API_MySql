using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using TST_API_MySql.Db;

namespace TST_API_MySql
{
    /// <summary>
    /// Defines the Startup class
    /// </summary>
    public class Startup
    {
        #region Private Properties

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        private IConfiguration Configuration { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="env">The env.</param>
        public Startup(IHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false)
                .AddEnvironmentVariables()
                .Build();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Configures the services.
        /// </summary>
        /// <param name="services">The services.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(p => p.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore );
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "TST_API_MySql", Version = "v1" }); });

            ConfigureDb(services);
        }

        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="env">The env.</param>
        /// <param name="db">The database.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EmployeeDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TST_API_MySql v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            db.RecreateDbBaseEntities();
        }

        #endregion

        #region Private Methods

        private void ConfigureDb(IServiceCollection services)
        {
            services.AddDbContext<EmployeeDbContext>(options => 
                options.UseMySql(Configuration.GetConnectionString("EmployeeDatabase"), new MySqlServerVersion(new Version(5, 7))));
        }

        #endregion
    }
}