using System.Collections.Generic;
using System.Linq;
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

        public Product FindWithCategory(int? id)
        {
            return _context.Set<Product>().Include(c => c.Category).SingleOrDefault(p => p.Id == id);
        }

        public IEnumerable<Product> ListWithCategories()
        {
            return _context.Set<Product>().Include(c => c.Category);
        }
    }
}
