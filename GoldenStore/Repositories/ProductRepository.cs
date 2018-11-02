using GoldenStore.Data;
using GoldenStore.Interfaces;
using GoldenStore.Models;

namespace GoldenStore.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
