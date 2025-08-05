using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Departments
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        Task<Department> FindDepartmentWithProducts(int Id);
        Task<List<Department>> GetAllDepartmentsWithProducts();
    }
}
