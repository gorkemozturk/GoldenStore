﻿using GoldenStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldenStore.Interfaces
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        List<OrderDetail> ListWithOrder(int id);
    }
}
