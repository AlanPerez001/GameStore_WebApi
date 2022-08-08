using GameStore_WebApi.ActionFilters;
using GameStore_WebApi.Authentications;
using GameStore_WebApi.Services;
using GameStore_WebApi.Services.Interfaces;
using GameStore_WebApi.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore_WebApi
{
    public class Startup
    {
        public static AppSettings respuestasApi { get; private set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ////Para poder aceptar archivos grandes en multipart form data
            services.Configure<FormOptions>(options =>
            {
                // Set the limit to 256 MB
                options.MultipartBodyLengthLimit = 268435456;
            });
            ////Para poder tener el validador de modelos personalizado
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });
            ////inyectar el validador de modelos 
            services.AddScoped<ModelValidatorFilter>();

            services.AddControllers();

            var connectionStrings = Configuration.GetSection("ConnectionStrings");
            services.Configure<ConnectionStrings>(connectionStrings);

            ////Para poder poner valores parametrizados en el appsettings.json. Este  archivo debe tener su dato de AppSettings
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            respuestasApi = appSettings;

            ////Parte para configurar los JWT
            var keyJWT = Encoding.ASCII.GetBytes(appSettings.KEYJWT);
            services.AddAuthentication(d =>
            {
                d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(d =>
            {
                d.RequireHttpsMetadata = false;
                d.SaveToken = true;
                d.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyJWT),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            ////Parte para generar la configuracion de la documentacion del swagger
            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"WEB API {groupName}",
                    Version = groupName,
                    Description = "API",
                    Contact = new OpenApiContact
                    {
                        Name = "Web api",
                        Email = string.Empty,
                        Url = new Uri("https://www.google.com/"),
                    }
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                   {
                     new OpenApiSecurityScheme
                     {
                       Reference = new OpenApiReference
                       {
                         Type = ReferenceType.SecurityScheme,
                         Id = "Bearer"
                       }
                      },
                      new string[] { }
                    }
                });


                var xmlPath = Path.Combine(AppContext.BaseDirectory, "GameStore_WebApi.xml");
                options.IncludeXmlComments(xmlPath);

            });


            ////Cada servicio hay que inyectarlo
            services.AddTransient<IAutenticacionService, AutenticacionService>();
            services.AddTransient<ILogService, DbLogService>();
            services.AddTransient<ICatalogoService, CatalogoService>();

            ////Parte donde se inyectan los servicios del JWT
            services.AddSingleton<ITokenRefresher>(x =>
           new TokenRefresher(keyJWT, x.GetService<IJWTAuthenticationManager>(), x.GetService<ILogService>()
           , appSettings, x.GetService<IAutenticacionService>()));
            services.AddSingleton<IRefreshTokenGenerator, RefreshTokenGenerator>();
            services.AddSingleton<IJWTAuthenticationManager>(x =>
                new JWTAuthenticationManager(appSettings.KEYJWT, x.GetService<IRefreshTokenGenerator>(),
                x.GetService<ILogService>(), appSettings, x.GetService<IAutenticacionService>()));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                //app.UseStatusCodePagesWithReExecute("/error/{0}");
                //app.UseExceptionHandler("/error/500");
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WB API V1");
            });


            app.UseErrorWrappingMiddleware();
            app.UseLogMiddleware();

            app.UseStaticFiles();


            app.UseStatusCodePagesWithReExecute("/error/{0}");
            app.UseExceptionHandler("/error/500");

            //app.UseHttpsRedirection();


            var supportedCultures = new[] { new CultureInfo("es-MX") };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("es-MX"),
                // Formatting numbers, dates, etc.
                SupportedCultures = supportedCultures,
                // UI strings that we have localized.
                SupportedUICultures = supportedCultures
            });




            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
