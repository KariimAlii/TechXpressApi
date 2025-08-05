using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Departments
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DepartmentRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Department> FindDepartmentWithProducts(int Id)
        {
            return await _dbContext.Departments
                .Include(d => d.Products)
                .FirstOrDefaultAsync(d => d.Id == Id);
        }

        public async Task<List<Department>> GetAllDepartmentsWithProducts()
        {
            var result = await GetWith
            (
                new string[] { "Products" }, 
                department => department.Name.StartsWith("M")
            );

            return result.ToList();
        }
    }
}
