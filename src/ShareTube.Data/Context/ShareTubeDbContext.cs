using ShareTube.Core;
using ShareTube.Core.Configuration;
using ShareTube.Core.Context;
using ShareTube.Core.Data;
using ShareTube.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading;
using log4net;

namespace ShareTube.Data.Context
{
    public class ShareTubeDbContext : DbContext, IDbContext
    {
        public virtual DbSet<PlayerStatus> PlayerStatuses { get; set; } = null!;
        public virtual DbSet<Room> Rooms { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserConnection> UserConnections { get; set; } = null!;
        public virtual DbSet<Video> Videos { get; set; } = null!;

        private IUserInformation? UserInformation { get; }

        public ShareTubeDbContext(DbContextOptions<ShareTubeDbContext> options)
            : base(options)
        {
            ChangeTracker.AutoDetectChangesEnabled = false;
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries<IEntity>();

            //audit fields
            foreach (var entry in entries)
            {
                if (!(entry.Entity is IEntity entity))
                    break;

                if (entry.State == EntityState.Added)
                {
                    entity.CreateDate = DateTime.UtcNow;
                    entity.CreatedBy = UserInformation?.UserName ?? string.Empty;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entity.UpdateDate = DateTime.UtcNow;
                    entity.UpdatedBy = UserInformation?.UserName;
                }
            }

            TruncateStringForChangedEntities(this);

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(ShareTubeDbContext)));
        }

        void IDbContext.Add(object entity)
        {
            base.Add(entity);
        }

        void IDbContext.Update(object entity)
        {
            base.Update(entity);
        }

        void IDbContext.Remove(object entity)
        {
            base.Remove(entity);
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> filter)
            where T : class, IEntity
        {
            return base.Set<T>().Where(filter).AsQueryable();
        }

        public string GetTableName<T>()
            where T : class, IEntity
        {
            return base.Model.FindEntityType(typeof(T)).GetTableName();
        }

        public void Execute(FormattableString sql)
        {
            //not supported by in-memory database
            if (!IsInMemoryDb)
            {
                var rawSql = string.Format(sql.Format, sql.GetArguments());
                base.Database.ExecuteSqlRaw(rawSql);
            }
            //base.Database.ExecuteSqlInterpolated(sql); //throws some @p0 exception - DJL 2/4/2020
        }

        private bool IsInMemoryDb => Database.ProviderName == "Microsoft.EntityFrameworkCore.InMemory";

        public bool SupportsJson() => !IsInMemoryDb;
        public bool SupportsBulk() => !IsInMemoryDb;

        private readonly static ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //https://devhow.net/2019/01/17/entity-framework-core-truncating-strings-based-on-length-constraint/
        public static void TruncateStringForChangedEntities(DbContext context)
        {
            var stringPropertiesWithLengthLimitations = context.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.ClrType == typeof(string))
                .Select(z => new
                {
                    StringLength = z.GetMaxLength(),
                    ParentName = z.DeclaringEntityType.Name,
                    PropertyName = z.Name
                })
                .Where(d => d.StringLength.HasValue);


            var editedEntitiesInTheDbContextGraph = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
                .Select(x => x.Entity);


            foreach (var entity in editedEntitiesInTheDbContextGraph)
            {
                var entityFields = stringPropertiesWithLengthLimitations.Where(d => d.ParentName == entity.GetType().FullName);

                foreach (var property in entityFields)
                {
                    var prop = entity.GetType().GetProperty(property.PropertyName);

                    if (prop == null)
                        continue;

                    var originalValue = prop.GetValue(entity) as string;
                    if (originalValue == null)
                        continue;

                    if (originalValue.Length > property.StringLength)
                    {
                        var entityTyped = entity as IEntity;
                        _log.Debug($"Entity '{entity.GetType().Name}':{entityTyped?.Id} Had value truncated from {originalValue.Length} to {property.StringLength} on property '{property.PropertyName}'");
                        prop.SetValue(entity, originalValue.Substring(0, property.StringLength.Value));
                    }
                }
            }
        }
    }

    public class ShareTubeDbContextFactory : IDesignTimeDbContextFactory<ShareTubeDbContext>
    {
        /// <summary>
        /// this is the endpoint used in Package Manager console. Change this to update a different database
        /// </summary>
        public ShareTubeDbContext CreateDbContext(string[] args)
        {
            var connString = "Data Source=localhost;Initial Catalog=ShareTube;Trusted_Connection=True;";
            var optionsBuilder = new DbContextOptionsBuilder<ShareTubeDbContext>();
            optionsBuilder.UseSqlServer(connString);

            return new ShareTubeDbContext(optionsBuilder.Options);
        }
    }
}
