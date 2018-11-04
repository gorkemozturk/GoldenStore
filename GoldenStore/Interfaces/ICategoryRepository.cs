using GoldenStore.Models;
using System.Collections.Generic;

namespace GoldenStore.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        IEnumerable<Category> ListParentCategories();
    }
}
