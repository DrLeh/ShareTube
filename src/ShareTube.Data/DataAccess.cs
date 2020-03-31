using ShareTube.Core.Configuration;
using ShareTube.Core.Data;
using ShareTube.Core.Loaders;
using ShareTube.Core.Models;
using ShareTube.Core.Security;
using ShareTube.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShareTube.Data
{
    public class DataAccess : IDataAccess
    {
        private readonly ShareTubeDbContext _dbContext;
        private readonly ISecurityPolicy _securityPolicy;

        public DataAccess(ShareTubeDbContext dbContext, ISecurityPolicy securityPolicy)
        {
            _dbContext = dbContext;
            _securityPolicy = securityPolicy;

            if (_securityPolicy == null)
                throw new ArgumentNullException(nameof(securityPolicy));
        }

        public IDataTransaction CreateTransaction()
        {
            return new DataTransaction(this);
        }

        private IQueryable<T> Query_Internal<T>() where T : class, IEntity
        {
            var q = _dbContext.Set<T>().AsQueryable();

            q = q.Where(x => !x.IsDeleted);

            return q;
        }

        private IQueryable<T> Query_Internal<T>(params Expression<Func<T, object>>[] includes) where T : class, IEntity
        {
            var q = Query_Internal<T>();

            foreach (var i in includes)
                q = q.Include(i);

            return q;
        }

        public IQueryable<T> Query<T>() where T : Entity
        {
            return _securityPolicy.Filter(Query_Internal<T>());
        }

        public IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : Entity
        {
            return _securityPolicy.Filter(Query_Internal(includes));
        }

        public IQueryable<T> Include<T, TP1>(IQueryable<T> query, Expression<Func<T, TP1>> expr1) where T : class
        {
            return query.Include(expr1);
        }

        public IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, TP1>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class
        {
            return query.Include(expr1).ThenInclude(expr2);
        }

        public IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, ICollection<TP1>>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class
        {
            return query.Include(expr1).ThenInclude(expr2);
        }



        public void ExecuteCommands(IReadOnlyList<IStoreCommand> commands)
        {
            foreach (var command in commands)
            {
                command.Execute(_dbContext);
            }

            _dbContext.SaveChanges();
        }
    }
}
