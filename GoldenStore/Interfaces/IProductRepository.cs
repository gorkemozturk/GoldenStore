using GoldenStore.Models;
using System.Collections.Generic;

namespace GoldenStore.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> ListWithCategories();
        Product FindWithCategory(int? id);
    }
}
