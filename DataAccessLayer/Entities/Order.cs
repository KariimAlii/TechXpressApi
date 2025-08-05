using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    // I. Configuration using Data Annotations
    [Table("Orders")]
    public class Order // You are free to choose any class name
    {
        // Naming Convention (2) : Primary Key
        // ➡️➡️    id , Id , ID
        // ➡️➡️    {class}id , {class}Id , {class}ID  ===> OrderId
        public int Id { get; set; }                   // ✅✅ PK
        //public int OrderId { get; set; }            // ✅✅ PK
        public int? Rating { get; set; }
        public string? Review { get; set; }
        // Convention (3) : Column_Name = Property_Name
        public DateTime Date { get; set; }
        //[ForeignKey("Customer")]
        [ForeignKey(nameof(Customer))]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } // Navigation Property
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>(); // Navigation Property
    }
}
