using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GoldenStore.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Category FindWithParent(int? id)
        {
            return _context.Set<Category>().Include(c => c.Parent).SingleOrDefault(m => m.Id == id);
        }
    }
}
