using DataAccessLayer.Configurations;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace DataAccessLayer.Context
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        // Conventions
        // Configuration                Higher Priority
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; } // Naming Convention ( 1 ) ===> Collection Name = Table Name
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Department> Departments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server = KARIM-ALI\\MSSQLSERVER01; Database = Ecommerce_System_DEPI_SecondRound; Integrated Security = SSPI; TrustServerCertificate = True";

                optionsBuilder
                        .UseSqlServer(connectionString)
                        .LogTo(Console.WriteLine, LogLevel.Information);
            }

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 🚩🚩 Configuration using Fluent API


            // 1- Table Name
            //modelBuilder.Entity<Product>()
            //    .ToTable("Products");

            // 2- Primary Key

            //modelBuilder.Entity<Product>()
            //    .HasKey(p => p.Id);

            //// 3- Column Data Type
            //modelBuilder.Entity<Product>()
            //    .Property(p => p.Price)
            //    .HasColumnType("decimal(18,2)");

            //// 4- Relationships

            //// Each Product belongs to One Department
            //// Each Department has Many Products
            //// Product  (M)    ---------    (1)    Department
            //modelBuilder.Entity<Product>()
            //    .HasOne(p => p.Department)
            //    .WithMany(d => d.Products)
            //    .HasForeignKey(p => p.DepartmentId);

            //// Each Product has Many OrderProducts
            //// Each OrderProduct has One Product
            //// Product  (1)    ---------    (M)    OrderProducts
            //modelBuilder.Entity<Product>()
            //    .HasMany(p => p.OrderProducts)
            //    .WithOne(op => op.Product)
            //    .HasForeignKey(op => op.ProductId);

            //modelBuilder.Entity<OrderProduct>()
            //    .HasKey(op => new { op.OrderId, op.ProductId });

            // Individual Calls
            //=============================

            //modelBuilder.ApplyConfiguration(new CustomerConfiguration());
            //modelBuilder.ApplyConfiguration(new ProductConfiguration());
            //modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderConfiguration());
            //modelBuilder.ApplyConfiguration(new OrderProductConfiguration());

            //new CustomerConfiguration().Configure(modelBuilder.Entity<Customer>());
            //new ProductConfiguration().Configure(modelBuilder.Entity<Product>());
            //new DepartmentConfiguration().Configure(modelBuilder.Entity<Department>());
            //new OrderConfiguration().Configure(modelBuilder.Entity<Order>());
            //new OrderProductConfiguration().Configure(modelBuilder.Entity<OrderProduct>());

            // Call from Assembly
            //=====================

            modelBuilder.ApplyConfigurationsFromAssembly
            (
                typeof(ApplicationDbContext).Assembly
            );

            base.OnModelCreating(modelBuilder);
        }
    }
}
