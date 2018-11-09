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
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<OrderDetail> ListWithOrder(int id)
        {
            return _context.Set<OrderDetail>().Where(o => o.OrderId == id).Include(o => o.Product).ToList();
        }
    }
}
