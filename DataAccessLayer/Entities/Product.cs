using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLayer.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }

        [ForeignKey(nameof(Department))]
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

        // Lazy Loading
        //==================

        // EF Core   load  entity properties  except  Navigation Properties

        // To load Navigation Property ===> LEFT JOIN

        // _dbContext.Products.FirstOrDefaultAsync(p => p.Id == 2);

        // Product.Department    ======SQL========>        Select ✅,✅,✅ FROM Product WHERE Product.Id = 2


        // Eager Loading
        //===================

        // EF Core   load  entity with  Navigation Properties ( Include )

        // _dbContext.Products
        // .Include(p => p.Department)
        // .FirstOrDefaultAsync(p => p.Id == 2);
        // Product.Department    ======SQL========>        Select ✅,✅,✅ FROM Product LEFT JOIN Department

        // _dbContext.Products
        // .Include(p => p.OrderProducts)
        // .FirstOrDefaultAsync(p => p.Id == 2);
        // Product.OrderProducts    ======SQL========>        Select ✅,✅,✅ FROM Product LEFT JOIN OrderProducts
    }
}
