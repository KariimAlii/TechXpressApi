
using BusinessLayer.Managers.Customers;
using BusinessLayer.Managers.Orders;
using BusinessLayer.Managers.Products;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories.Customers;
using DataAccessLayer.Repositories.Departments;
using DataAccessLayer.Repositories.Generic;
using DataAccessLayer.Repositories.Orders;
using DataAccessLayer.Repositories.Products;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.Api.Middlewares;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using DataAccessLayer.Entities;
using Microsoft.OpenApi.Models;
using Serilog;
using PresentationLayer.Api.Filters;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;

namespace PresentationLayer.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt Authorization header using the bearer schema (Example : 'Bearer {token}')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
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
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Host.UseSerilog((context, serviceProvider, configuration) =>
            {
                configuration
                    .ReadFrom.Configuration(context.Configuration)
                    .Enrich.FromLogContext();
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<ICustomOrderRepo, CustomOrderRepo>();

            builder.Services.AddScoped<IProductManager, ProductManager>();
            builder.Services.AddScoped<ICustomerManager, CustomerManager>();
            builder.Services.AddScoped<IOrderManager, OrderManager>();

            // Filters
            builder.Services.AddScoped<LogExecutionTimeFilter>();
            builder.Services.AddScoped<LogExecutionTimeFilterAsync>();
            builder.Services.AddScoped<IPWhiteListAuthorizationFilter>();

            var cacheDuration = builder.Configuration["Redis:DurationInMinutes"];
            int? redisCachingDurationInMinutes = cacheDuration != null ? Convert.ToInt32(cacheDuration) : null;

            builder.Services.AddScoped<RedisCacheResourceFilter>(provider =>
            {
                var cache = provider.GetRequiredService<IDistributedCache>();
                return new RedisCacheResourceFilter(cache, redisCachingDurationInMinutes!.Value);
            });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Redis Caching
            builder.Services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = builder.Configuration["Redis:Configuration"];
            });



            var connectionString = builder.Configuration.GetConnectionString("EcommerceApplication");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information);
            });

            builder.Services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<ApplicationDbContext>();


            builder.Services.AddAuthentication(options =>
            {
                // USE JWT Bearer token
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;   // require https
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // expiration (exp)
                    ValidateLifetime = true,

                    ValidateIssuer = true,
                    //ValidIssuer = "http://localhost:4500"
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],


                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["Jwt:Audience"],


                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"])),    // secret (string)  --->  byte[] ---> Symmetric Key
                };
            });

            var myPolicty = "MyPolicy";

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: myPolicty, policy =>
                {
                    //policy
                    //.AllowAnyOrigin()
                    //.AllowAnyMethod()
                    //.AllowAnyHeader();

                    policy
                        .WithOrigins("http://127.0.0.1:5500")
                        .WithMethods("Get", "Post");
                });
            });
            var app = builder.Build();

            app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseCors(myPolicty);

            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.UseSerilogRequestLogging();

            app.Run();
        }
    }
}
