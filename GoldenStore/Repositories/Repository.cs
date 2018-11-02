using GoldenStore.Data;
using GoldenStore.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GoldenStore.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Create(T entity)
        {
            _context.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
            _context.SaveChanges();
        }

        public T Find(int? id)
        {
            return _context.Set<T>().Find(id);
        }

        public T Find(Func<T, bool> predicate)
        {
            return _context.Set<T>().Where(predicate).SingleOrDefault();
        }

        public IEnumerable<T> List()
        {
            return _context.Set<T>();
        }

        public void Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
