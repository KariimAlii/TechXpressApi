using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Orders
{
    public class CustomOrderRepo : ICustomOrderRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public CustomOrderRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task UpdateOrderRatingAndReview(Order order, int rating, string review)
        {
            order.Rating = rating;
            order.Review = review;

            // Change Tracker

            var trackedEntries = _dbContext.ChangeTracker.Entries().ToList();

            foreach (var entry in trackedEntries)
            {
                Console.WriteLine($"Entity : {entry.Entity.GetType().Name}, State: {entry.State}");
            }

            var entry2 = _dbContext.Entry(order);

            Console.WriteLine($"Entity : {entry2.Entity.GetType().Name}, State: {entry2.State}");
        }
    }
}
