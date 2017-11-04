using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShareTube.Core.Data
{

    public interface IDataAccess : IDisposable
    {
        IQueryable<T> QueryEntities<T>() where T : Entity;
        T Find<T>(Expression<Func<T, bool>> finder) where T : Entity;
        void Add<T>(T entity) where T : Entity;
        void Remove<T>(T entity) where T : Entity;
        void Remove<T>(Expression<Func<T, bool>> finder) where T : Entity;
        void Commit();
    }
}
