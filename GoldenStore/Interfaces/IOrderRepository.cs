using GoldenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        List<Order> ListRelatedWithUser(string id);
    }
}
