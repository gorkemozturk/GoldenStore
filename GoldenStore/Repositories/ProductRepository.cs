using System.Collections.Generic;
using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using Microsoft.EntityFrameworkCore;

namespace GoldenStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> ListWithCategories()
        {
            return _context.Set<Product>().Include(c => c.Category);
        }
    }
}
