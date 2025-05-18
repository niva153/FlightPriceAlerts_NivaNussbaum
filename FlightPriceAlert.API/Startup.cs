
using FlightPriceAlert.API.Middlewares;
using FlightPriceAlert.Application.Interfaces;
using FlightPriceAlert.Application.Services;
using FlightPriceAlert.Infrastructure.Data;
using FlightPriceAlert.Infrastructure.Interfaces;
using FlightPriceAlert.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace FlightPriceAlert.API
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
            // Add database connection
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // Add controllers
            services.AddControllers();

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add repositories
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Configure AlertService based on environment
            services.AddScoped<IAlertService>(provider =>
            {
                var env = provider.GetService<IHostEnvironment>();
                if (env != null && env.IsDevelopment())
                {
                    return provider.GetService<MockAlertService>() ??
                           throw new InvalidOperationException("Failed to resolve MockAlertService");
                }
                else
                {
                    return provider.GetService<AlertService>() ??
                           throw new InvalidOperationException("Failed to resolve RealAlertService");
                }
            });

            // Register both services so they can be resolved by DI
            services.AddScoped<MockAlertService>();
            services.AddScoped<AlertService>();

            // Add authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key not configured")))
                };
            });

            // Add CORS
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins(Configuration["AllowedOrigins"]?.Split(',') ?? Array.Empty<string>())
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            // Add Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Flight Price Alert API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Flight Price Alert API v1"));
                app.UseFakeUser();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors("AllowSpecificOrigin");

            if (!env.IsDevelopment())
            {
                app.UseAuthentication();
            }
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
