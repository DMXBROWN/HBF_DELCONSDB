using System;
using System.IO.Compression;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SnapObjects.Data;
using SnapObjects.Data.SqlServer;
using SnapObjects.Data.AspNetCore;
using DWNet.Data.AspNetCore;
using delconsdb_api.Services;
using delconsdb_api.Services.Impl;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace delconsdb_api 
{
    public class Startup
    {
        public static string SECRET { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(m =>
            {
                m.UseCoreIntegrated();
                m.UsePowerBuilderIntegrated();
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // Uncomment the following line to connect to the SQL server database.
            // Note: Replace "ContextName" with the configured context name; replace "key" with the database connection name that exists in appsettings.json. The sample code is as follows:
             services.AddDataContext<DelConsDBDataContext>(m => m.UseSqlServer(Configuration["ConnectionStrings:MAS_DEL_CONSDB"]));

            // Uncomment the following line to connect to the Oracle database.
            // Note: Replace "ContextName" with the configured context name; replace "key" with the database connection name that exists in appsettings.json. The sample code is as follows:
            // services.AddDataContext<ContextName>(m => m.UseOracle(Configuration["ConnectionStrings:key"]));

            // Uncomment the following line to connect to the PostGreSql database.
            // Note: Replace "ContextName" with the configured context name; replace "key" with the database connection name that exists in appsettings.json. The sample code is as follows:
            // services.AddDataContext<ContextName>(m => m.UsePostgreSql(Configuration["ConnectionStrings:key"]));

            // Uncomment the following line to connect to the ODBC database.
            // Note: Replace "ContextName" with the configured context name; replace "key" with the database connection name that exists in appsettings.json. The sample code is as follows:
            // services.AddDataContext<ContextName>(m => m.UseOdbc(Configuration["ConnectionStrings:key"]));
            
            services.AddGzipCompression(CompressionLevel.Fastest);

            SECRET = Configuration["Token:Key"];

            var key = Encoding.ASCII.GetBytes(SECRET);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = true;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ISalesOrderService, SalesOrderService>();
            services.AddScoped<IDeliveryNoteService, DeliveryNoteService>();
            //test changes   
           // Arjun

        }

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

            // Adds middleware for redirecting HTTP Requests to HTTPS.
            // app.UseHttpsRedirection();

            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseResponseCompression();

            app.UseMvc();

            app.UseDataWindow();
        }
    }
}