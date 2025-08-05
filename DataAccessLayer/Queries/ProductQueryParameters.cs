using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Queries
{
    public class ProductQueryParameters
    {
        public string? Name { get; set; }
        public string? DepartmentName { get; set; }
        public string? SortBy { get; set; } = "Name";
        public bool SortDesc { get; set; } = false;

        #region Pagination is optional
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }
        #endregion

        #region Pagination is required
        //public int PageNo { get; set; } = 1;
        //public int PageSize { get; set; } = 10;
        #endregion
    }
}
