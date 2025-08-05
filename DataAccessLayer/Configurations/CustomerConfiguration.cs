using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(c => c.Address)
                .HasMaxLength(100);

            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(15);

            builder
                .HasMany(c => c.Orders)
                .WithOne(order => order.Customer)
                .HasForeignKey(order => order.CustomerId);

            builder.HasData(GetInitialCustomerData());
        }

        public static List<Customer> GetInitialCustomerData()
        {
            return new List<Customer>
            {
                new Customer {Id = 1, Name = "Essam", Address = "Sheikh Zayed", PhoneNumber = "01255789444"},
                new Customer {Id = 2, Name = "Amr", Address = "Nasr City", PhoneNumber = "01255789444"},
                new Customer {Id = 3, Name = "Sara", Address = "Mokattam", PhoneNumber = "01255789444"}
            };
        }
    }
}
