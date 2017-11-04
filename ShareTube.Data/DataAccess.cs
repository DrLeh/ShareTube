using Microsoft.EntityFrameworkCore;
using ShareTube.Core.Data;
using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShareTube.Data
{
    public class DataAccess : IDataAccess
    {
        DbContext _context;
        DbContext Context => _context ?? (_context = new ShareTubeDataContext());

        public void Dispose()
        {
            _context?.Dispose();
        }

        public IQueryable<T> QueryEntities<T>() where T : Entity
        {
            return Context.Set<T>().AsQueryable();
        }

        public T Find<T>(Expression<Func<T, bool>> finder) where T : Entity
        {
            return QueryEntities<T>().SingleOrDefault(finder);
        }

        public void Add<T>(T entity) where T : Entity
        {
            Context.Set<T>().Add(entity);
        }

        public void Remove<T>(T entity) where T : Entity
        {
            Context.Set<T>().Add(entity);
        }

        public void Remove<T>(Expression<Func<T, bool>> finder) where T : Entity
        {
            var entities = QueryEntities<T>().Where(finder).ToList();
            foreach (var e in entities)
                Remove(e);
        }

        public void Commit()
        {
            Context.SaveChanges();
        }
    }
}
