﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ClassTranscribeDatabase;
using ClassTranscribeDatabase.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authorization;
using ClassTranscribeServer.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Swashbuckle.AspNetCore.Filters;
using System.Collections.Generic;
using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using ClassTranscribeServer.Utils;

namespace ClassTranscribeServer
{
    public class Startup
    {
        public Startup(IOptions<AppSettings> appSettings)
        {
            Globals.appSettings = appSettings.Value;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
            services.AddDbContext<CTDbContext>(options =>
                options.UseLazyLoadingProxies().UseNpgsql(Globals.appSettings.POSTGRES));

            //// ===== Add Identity ========
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            }).AddRoles<IdentityRole>()
                .AddRoleManager<RoleManager<IdentityRole>>()
                .AddEntityFrameworkStores<CTDbContext>()
                .AddDefaultTokenProviders();
            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Globals.appSettings.JWT_ISSUER,
                        ValidAudience = Globals.appSettings.JWT_ISSUER,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Globals.appSettings.JWT_KEY)),
                        ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Authorization handlers.
            services.AddScoped<IAuthorizationHandler,
                                  ReadOfferingAuthorizationHandler>();
            services.AddScoped<IAuthorizationHandler,
                                  UpdateOfferingAuthorizationHandler>();

            // Configure your policies
            services.AddAuthorization(options =>
            {
                options.AddPolicy(Globals.POLICY_UPDATE_OFFERING, policy =>
                    policy.Requirements.Add(new UpdateOfferingRequirement()));
                options.AddPolicy(Globals.POLICY_READ_OFFERING,
                  policy => policy.AddRequirements(new ReadOfferingRequirement()));
            });


            // Register the Swagger generator, defining 1 or more Swagger documents
            // new ApiKeyScheme { };
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "ClassTranscribeServer API"
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer",
                new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please enter into field the word 'Bearer' following by space and JWT",
                    Name = "Authorization",
                    Type = "apiKey"
                });
                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>> {{ "Bearer", Enumerable.Empty<string>() }});
                c.OperationFilter<FileUploadOperation>(); //Register File Upload Operation Filter
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, CTDbContext dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseCors(MyAllowSpecificOrigins);
            // app.UseHttpsRedirection();
            app.UseAuthentication();
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                FileProvider = new PhysicalFileProvider(Globals.appSettings.DATA_DIRECTORY),
                RequestPath = "/Data"
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "swag";
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute("default","/swag/index.html");
            });
            Seeder.Seed(dbContext);
        }
    }
}
