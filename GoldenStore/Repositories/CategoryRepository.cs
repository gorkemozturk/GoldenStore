using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;

namespace GoldenStore.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
