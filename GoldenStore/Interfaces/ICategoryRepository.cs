using GoldenStore.Models;

namespace GoldenStore.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category FindWithParent(int? id);
    }
}
