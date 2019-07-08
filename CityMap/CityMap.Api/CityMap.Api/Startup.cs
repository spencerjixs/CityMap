using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityMap.Models;
using CityMap.Models.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using CityMap.Api.Interfaces;
using CityMap.DataAccess.Interfaces;
using CityMap.DataAccess.Managers;
using CityMap.Api.Managers;

namespace CityMap.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        readonly string CityMapAllowSpecificOrigins = "_cityAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //SQL Server DB Context
            //services.AddDbContext<CityMapSqlDbContext>(option =>
            //option.UseSqlServer(Configuration.GetConnectionString("CityMapSqlDB")));

            services.AddDbContext<CityMapSqlDbContext>(opt => opt.UseInMemoryDatabase("CityMapSqlDB"));

            //configure strongly settings objects
            services.Configure<AppSettings>(
            options =>
            {
                options.JWTAuthenticationSecret = Configuration.GetSection("AppSettings:JWTAuthenticationSecret").Value;
                options.GoogleMapApiKey = Configuration.GetSection("AppSettings:GoogleMapApiKey").Value;
                options.OpenWeatherApiKey = Configuration.GetSection("AppSettings:OpenWeatherApiKey").Value;
                options.OpenWeatherApiUrl = Configuration.GetSection("AppSettings:OpenWeatherApiUrl").Value;
                options.TimeZoneApiUrl = Configuration.GetSection("AppSettings:TimeZoneApiUrl").Value;
                options.ElevationApiUrl = Configuration.GetSection("AppSettings:ElevationApiUrl").Value;
                options.AngularClientUrl = Configuration.GetSection("AppSettings:AngularClientUrl").Value;
            });
            var appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();

            // Add services to allow cors for Angular SPA Url.
            var angularClientUrl = appSettings.AngularClientUrl;
            services.AddCors(options =>
            {
                options.AddPolicy(CityMapAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins(angularClientUrl);
                });
            });
            services.AddHttpClient();


            //Add SwaggerGen
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "City Map API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });
            });


            // configure jwt authentication
            
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.JWTAuthenticationSecret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var adminManager = context.HttpContext.RequestServices.GetRequiredService<IAdminManager>();
                        string userNameAndPassword = context.Principal.Identity.Name;
                        if (!userNameAndPassword.Contains("|"))
                        {
                            context.Fail("Unauthorized");
                        }
                        string[] comKeys = userNameAndPassword.Split('|');
                        string userName = comKeys[0];
                        string password = comKeys[1];
                        var user = adminManager.UserLogin(userName, password);
                        if (user == null)
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services.AddScoped<IAdminDataManager, AdminDataManager>();
            services.AddScoped<ICityMapDataManager, CityMapDataManager>();
            services.AddScoped<IAdminManager, AdminManager>();
            services.AddScoped<ICityMapManager, CityMapManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "City Map API V1");
            });
            //"/swagger/index.html"
            app.UseAuthentication();

            app.UseCors(CityMapAllowSpecificOrigins);
            app.UseMvc();
        }
    }
}
