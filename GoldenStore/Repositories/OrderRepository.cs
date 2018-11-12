using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<Order> ListOrders()
        {
            return _context.Set<Order>().Include(o => o.ApplicationUser).OrderByDescending(o => o.CreatedAt).ToList();
        }

        public List<Order> ListRelatedWithUser(string id)
        {
            return _context.Set<Order>().Where(o => o.ApplicationUserId == id).OrderByDescending(o => o.CreatedAt).ToList();
        }
    }
}
