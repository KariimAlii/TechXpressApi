using TPH.DataAccessLayer.Context;
using TPH.DataAccessLayer.Entities;

namespace TPH
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var admin = new Admin
            {
                Fname = "Maher",
                Lname = "Ali",
                SSN = 123,
                Manager = "Ashraf"
            };

            var dev = new Developer
            {
                Fname = "Ibrahim",
                Lname = "Mohamed",
                Salary = 12000,
                JobTitle = "Junior"
            };

            using (var context = new ApplicationDbContext())
            {
                context.Employees.Add(admin);
                context.Employees.Add(dev);
                context.SaveChanges();
            }

            using (var context = new ApplicationDbContext())
            {
                Console.WriteLine("Admins are : ");

                foreach (var item in context.Set<Employee>().OfType<Admin>())
                {
                    Console.WriteLine(item);
                }

                Console.WriteLine("Developer are : ");
                foreach (var item in context.Set<Employee>().OfType<Developer>())
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
