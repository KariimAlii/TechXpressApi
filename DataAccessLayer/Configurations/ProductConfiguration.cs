using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            // 1- Table Name
            builder
                .ToTable("Products");

            // 2- Primary Key

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Id)
                .ValueGeneratedNever();

            builder.Property(p => p.Name)
                .IsRequired()
                .HasMaxLength(100);
                

            builder.Property(p => p.Image)
                .HasMaxLength(250);

            builder.Property(p => p.Description)
                .HasColumnType("nvarchar(max)");
            // 3- Column Data Type
            builder
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            // 4- Relationships

            // Each Product belongs to One Department
            // Each Department has Many Products
            // Product  (M)    ---------    (1)    Department
            builder
                 .HasOne(p => p.Department)
                 .WithMany(d => d.Products)
                 .HasForeignKey(p => p.DepartmentId);

            // Each Product has Many OrderProducts
            // Each OrderProduct has One Product
            // Product  (1)    ---------    (M)    OrderProducts
            builder
                .HasMany(p => p.OrderProducts)
                .WithOne(op => op.Product)
                .HasForeignKey(op => op.ProductId);
        }
    }
}
