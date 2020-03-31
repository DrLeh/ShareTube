using AutoMapper;
using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShareTube.Core.Data
{
    public interface IDataAccess
    {
        IDataTransaction CreateTransaction();
        void ExecuteCommands(IReadOnlyList<IStoreCommand> commands);
        IQueryable<T> Include<T, TP1>(IQueryable<T> query, Expression<Func<T, TP1>> expr1) where T : class;
        IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, TP1>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class;
        IQueryable<T> Include<T, TP1, TP2>(IQueryable<T> query, Expression<Func<T, ICollection<TP1>>> expr1, Expression<Func<TP1, TP2>> expr2) where T : class;
        IQueryable<T> Query<T>() where T : Entity;
        IQueryable<T> Query<T>(params Expression<Func<T, object>>[] includes) where T : Entity;
    }
}
