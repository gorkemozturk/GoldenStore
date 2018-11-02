using System;
using System.Collections.Generic;

namespace GoldenStore.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> List();
        T Find(int? id);
        T Find(Func<T, bool> predicate);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Save();
    }
}
