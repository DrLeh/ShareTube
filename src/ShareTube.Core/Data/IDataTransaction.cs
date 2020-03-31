using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ShareTube.Core.Data
{
    public interface IDataTransaction
    {
        void Add<T>(T entity) where T : IEntity;
        void Update<T>(T entity) where T : IEntity;
        void Remove<T>(T entity) where T : IEntity;
        void BulkUpdate<T>(Expression<Func<T, bool>> selector, Expression<Func<T, object>> obj) where T : class, IEntity;
        void SoftDelete<T>(Expression<Func<T, bool>> selector) where T : class, IEntity;
        void BulkRemove<T>(Expression<Func<T, bool>> selector) where T : class, IEntity;
        void Commit();
        IReadOnlyList<IStoreCommand> Commands { get; }
    }

    public enum StoreInteractionType
    {
        Added,
        Updated,
        Removed,
    }

    public interface IDbContext
    {
        void Add(object entity);
        void Update(object entity);
        void Remove(object entity);
        IQueryable<T> Query<T>(Expression<Func<T, bool>> filter)
            where T : class, IEntity;
        void Execute(FormattableString sql);
        string GetTableName<T>()
            where T : class, IEntity;
        bool SupportsJson();
        bool SupportsBulk();
    }

    public interface IStoreCommand
    {
        StoreInteractionType Type { get; }
        void Execute(IDbContext context);
    }

    public interface IEntityStoreCommand : IStoreCommand
    {
        object Entity { get; }
    }

    public interface IEntityStoreCommand<out T> : IEntityStoreCommand
    {
        T EntityTyped { get; }
    }

    public interface IBulkStoreCommand : IStoreCommand
    {

    }

    public interface IBulkStoreCommand<T> : IBulkStoreCommand
    {
        Expression<Func<T, bool>> Selector { get; }
    }

    public interface ISqlStoreCommand<T> : IStoreCommand
        where T : class, IEntity
    {

    }
}
