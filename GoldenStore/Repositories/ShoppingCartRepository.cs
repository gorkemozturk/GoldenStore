using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Repositories
{
    public class ShoppingCartRepository : Repository<ShoppingCart>, IShoppingCartRepository
    {
        public ShoppingCartRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<ShoppingCart> ListWithUser(string id)
        {
            return _context.Set<ShoppingCart>().Where(c => c.ApplicationUserId == id).ToList();
        }

        public void RemoveCart(List<ShoppingCart> entity)
        {
            _context.RemoveRange(entity);
        }
    }
}
