using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using AutoMapper;
using EventAPI.BLL;
using EventAPI.DAL;
using EventAPI.DAL.Repositories;
using EventAPI.Domain.DataSeeder;
using EventAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

namespace EventAPI
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
            services.AddHttpContextAccessor();

            services.AddScoped<IDatabaseSeeder, DatabaseSeeder>();

            services.AddScoped<IEventBll, EventBll>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventCompanyRepository, EventCompanyRepository>();

            services.AddScoped<IAcademicYearBll, AcademicYearBll>();
            services.AddScoped<IAcademicYearRepository, AcademicYearRepository>();

            services.AddScoped<ICompanyBll, CompanyBll>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();

            services.AddScoped<IInternshipRepository, InternshipRepository>();
            services.AddScoped<IInternshipBll, InternshipBll>();

            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentBll, AppointmentBll>();

            services.AddScoped<IAttendeeRepository, AttendeeRepository>();
            services.AddScoped<IAttendeeBLL, AttendeeBLL>();

            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IStudentBLL, StudentBLL>();

            services.AddScoped<ISpecializationRepository, SpecializationRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IEnvironmentRepository, EnvironmentRepository>();
            services.AddScoped<IFileStorageService, FileStorageService>();
            
            services.AddDbContext<EventDBContext>(opt =>
            {
                opt.UseSqlServer(Configuration["ConnectionStrings:EventDB"]);
            });


            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddHttpClient("StagebeheerAPI", c =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();
                var bearerToken = httpContextAccessor.HttpContext.Request
                    .Headers["Authorization"]
                    .FirstOrDefault(h => h.StartsWith("bearer ", StringComparison.InvariantCultureIgnoreCase));
                if (bearerToken != null) c.DefaultRequestHeaders.Add("Authorization", bearerToken);

                c.BaseAddress = new Uri(Configuration.GetSection("InternshipService").GetValue<string>("API"));
            });

            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Events API",
                    Version = "v1"
                });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);

                options.IncludeXmlComments(xmlCommentsFullPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                services.AddControllersWithViews(options =>
                {
                    options.InputFormatters.Insert(0, GetJsonPatchInputFormatter());
                });
            });

            services.AddAutoMapper(typeof(Startup));


            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration.GetSection("Jwt").GetValue<string>("Issuer"),
                        ValidAudience = Configuration.GetSection("Jwt").GetValue<string>("Issuer"),
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt").GetValue<string>("Key")))
                    };
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("CompanyOnly", policy => policy.RequireClaim("roleCode", "COM"));
                options.AddPolicy("CoordinatorOnly", policy => policy.RequireClaim("roleCode", "COO"));
                options.AddPolicy("StudentOnly", policy => policy.RequireClaim("roleCode", "STU"));
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, EventDBContext dBContext,
            IServiceProvider serviceProvider)
        {
            IdentityModelEventSource.ShowPII = true;
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();
            app.UseCors("AllowAllOrigins");

            app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

            app.UseSwagger().UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("swagger/v1/swagger.json", "Events API V1");
                options.RoutePrefix = string.Empty;
            });

            var pendingMigrations = dBContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any()) dBContext.Database.Migrate();

            var databaseSeeder = serviceProvider.GetService<IDatabaseSeeder>();
            databaseSeeder.SeedData();
        }

        private static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
        {
            var builder = new ServiceCollection()
                .AddLogging()
                .AddMvc()
                .AddNewtonsoftJson()
                .Services.BuildServiceProvider();

            return builder
                .GetRequiredService<IOptions<MvcOptions>>()
                .Value
                .InputFormatters
                .OfType<NewtonsoftJsonPatchInputFormatter>()
                .First();
        }
    }
}