using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Name")]
        [Required]
        [MaxLength(200)]
        public string Name { get; set; }
        [Column("Address")]
        public string Address { get; set; }
        [Column("PhoneNumber")]
        public string PhoneNumber { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
