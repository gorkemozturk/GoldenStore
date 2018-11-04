using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace GoldenStore.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Category> ListParentCategories()
        {
            return _context.Set<Category>().Where(c => c.IsActive == true).Where(c => c.ParentId == null).ToList();
        }
    }
}
