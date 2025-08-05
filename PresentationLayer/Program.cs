using BusinessLayer.Managers.Customers;
using BusinessLayer.Managers.Orders;
using BusinessLayer.Managers.Products;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Customers;
using DataAccessLayer.Repositories.Departments;
using DataAccessLayer.Repositories.Generic;
using DataAccessLayer.Repositories.Products;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer
{
    public class Program
    {
        public static void Main(string[] args)   // Entry Point
        {
            #region Configure Application Settings
            var builder = WebApplication.CreateBuilder(args);

            // Builder Pattern

            // Add services to the container.
            builder.Services.AddControllersWithViews(); // .AddSessionStateTempDataProvider()

            var connectionString = builder.Configuration.GetConnectionString("EcommerceApplication");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                    .UseSqlServer(connectionString)
                    .LogTo(Console.WriteLine, LogLevel.Information);
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            builder.Services.AddScoped<IProductManager, ProductManager>();
            builder.Services.AddScoped<ICustomerManager, CustomerManager>();
            builder.Services.AddScoped<IOrderManager, OrderManager>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20); // configure lifetime of a session
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.HttpOnly = true;
            });


            #region Manual Authentication
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //        .AddCookie(options =>
            //        {
            //            options.ExpireTimeSpan = TimeSpan.FromHours(2);
            //            options.LoginPath = "/Account/Login";                      // Redirection Url if the user is not authenticated
            //            options.AccessDeniedPath = "/Account/Authorized";          // Redirection Url if the user is not authorized
            //        });
            //.AddOAuth()
            //.AddBearerToken() 
            #endregion

            #region Identity Authentication
            builder.Services.AddIdentity<User, Role>(options =>   // ==> AddIdentity<IdentityUser, IdentityRole>
            {
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>();
            //}).AddEntityFrameworkStores<IdentityDbContext>(); // ==> if you are not using custom db context

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
                options.LoginPath = "/Account/Login";
                options.AccessDeniedPath = "/Account/NotAuthorized";
            });
            #endregion
            var app = builder.Build();
            #endregion

            #region Pipeline
            //app.Use(async (context, next) =>
            //{
            //    // logic to be executed in the first direction

            //    await context.Response.WriteAsync("Middleware 1 \n");

            //    // call next
            //    await next.Invoke();

            //    // logic to be executed in the second direction
            //    await context.Response.WriteAsync("Middleware 1_1 \n");
            //});

            //app.Use(async (context, next) =>
            //{
            //    // logic to be executed in the first direction

            //    await context.Response.WriteAsync("Middleware 2 \n");

            //    // call next
            //    await next.Invoke();

            //    // logic to be executed in the second direction
            //    await context.Response.WriteAsync("Middleware 2_2 \n");
            //});

            //app.Run(async (context) =>
            //{
            //    await context.Response.WriteAsync("Terminate \n");
            //});

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            // 🚩🚩  is a middleware component in the ASP.NET Core request pipeline that:
            // ✅ Intercepts every request and checks if an authentication cookie is present.
            // ✅ If the cookie is present, it decrypts the cookie and reconstructs the user's identity (claims).
            // ✅ It attaches this identity to the (HttpContext.User) property, making the user's information (claims) available throughout the application.


            app.UseAuthorization();  // read user information from   (HttpContext.User)    ====>  (Role) Claim

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run(); 
            #endregion
        }
    }
}
