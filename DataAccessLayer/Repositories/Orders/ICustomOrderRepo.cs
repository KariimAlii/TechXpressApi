using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Orders
{
    public interface ICustomOrderRepo
    {
        Task UpdateOrderRatingAndReview(Order order, int rating, string review);
    }
}
