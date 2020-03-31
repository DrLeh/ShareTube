using ShareTube.Core.Data;
using ShareTube.Core.Models;
using ShareTube.Data.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using log4net;

namespace ShareTube.Data
{
    //here you'd implement this interface, and configure how those commands get executed
    // be it via Entity Framework, or what have you
    public class EntityStoreCommand<TModel> : IEntityStoreCommand
        where TModel : IEntity
    {
        public StoreInteractionType Type { get; }

        public object Entity { get; }

        public EntityStoreCommand(StoreInteractionType action, IEntity model)
        {
            Type = action;
            Entity = model;
        }

        public void Execute(IDbContext context)
        {
            switch (Type)
            {
                case StoreInteractionType.Added: context.Add(Entity); break;
                case StoreInteractionType.Updated: context.Update(Entity); break;
                case StoreInteractionType.Removed: context.Remove(Entity); break;
            }
        }
    }

    public class BulkRemoveCommand<TModel> : IBulkStoreCommand<TModel>
        where TModel : class, IEntity
    {
        public StoreInteractionType Type => StoreInteractionType.Removed;
        public Expression<Func<TModel, bool>> Selector { get; }

        public BulkRemoveCommand(Expression<Func<TModel, bool>> selector)
        {
            Selector = selector;
        }

        public void Execute(IDbContext context)
        {
            if (context.SupportsBulk())
            {
                context.Query(Selector).DeleteFromQuery();
            }
            else
            {
                // should only be needed for in memory integration tests, but causes the test not to fail.
                var toDelete = context.Query(Selector).ToList();
                foreach (var e in toDelete)
                    context.Remove(e);
            }
        }
    }

    public class BulkUpdateCommand<TModel> : IBulkStoreCommand<TModel>
        where TModel : class, IEntity
    {
        public StoreInteractionType Type => StoreInteractionType.Updated;
        public Expression<Func<TModel, bool>> Selector { get; }
        public Expression<Func<TModel, object>> UpdateExpression { get; }

        public BulkUpdateCommand(Expression<Func<TModel, bool>> selector, Expression<Func<TModel, object>> updateExpression)
        {
            Selector = selector;
            UpdateExpression = updateExpression;
        }

        public void Execute(IDbContext context)
        {
            if (context.SupportsBulk())
            {
                context.Query(Selector).UpdateFromQuery(UpdateExpression);
            }
            else
            {
                var entities = context.Query(Selector).ToList();
                foreach(var e in entities)
                {
                    UpdateExpression.Compile()(e);
                    context.Update(e);
                }
            }
        }
    }

    public class DataTransaction : IDataTransaction
    {
        public IReadOnlyList<IStoreCommand> Commands => _commands;
        private readonly DataAccess _dataAccess;
        private readonly List<IStoreCommand> _commands;
        private readonly List<Action> _beforeCommit;
        private readonly List<Action> _afterCommit;

        public DataTransaction(DataAccess dataAccess)
        {
            _dataAccess = dataAccess;
            _commands = new List<IStoreCommand>();
            _beforeCommit = new List<Action>();
            _afterCommit = new List<Action>();
        }

        public void Add<T>(T entity)
            where T : IEntity
        {
            _commands.Add(new EntityStoreCommand<T>(StoreInteractionType.Added, entity));
        }

        public void Update<T>(T entity)
            where T : IEntity
        {
            _commands.Add(new EntityStoreCommand<T>(StoreInteractionType.Updated, entity));
        }

        public void Remove<T>(T entity)
            where T : IEntity
        {
            _commands.Add(new EntityStoreCommand<T>(StoreInteractionType.Removed, entity));
        }

        public void BulkRemove<T>(Expression<Func<T, bool>> selector)
            where T : class, IEntity
        {
            _commands.Add(new BulkRemoveCommand<T>(selector));
        }

        public void BulkUpdate<T>(Expression<Func<T, bool>> selector, Expression<Func<T, object>> obj)
            where T : class, IEntity
        {
            _commands.Add(new BulkUpdateCommand<T>(selector, obj));
        }


        public void SoftDelete<T>(Expression<Func<T, bool>> selector)
            where T : class, IEntity
        {
            _commands.Add(new BulkUpdateCommand<T>(selector, x => new { IsDeleted = true }));
        }

        public void Commit()
        {
            _dataAccess.ExecuteCommands(_commands);
            _commands.Clear();
        }
    }
}
