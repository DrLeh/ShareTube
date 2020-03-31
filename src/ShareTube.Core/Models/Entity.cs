using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Models
{
    public interface IEntity
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Denotes that the entity is soft-deleted
        /// </summary>
        bool IsDeleted { get; set; }

        DateTime CreateDate { get; set; }
        string CreatedBy { get; set; }
        DateTime? UpdateDate { get; set; }
        string? UpdatedBy { get; set; }
    }

    public abstract class Entity : IEntity
    {
        /// <summary>
        /// Primary Key
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Denotes that the entity is soft-deleted
        /// </summary>
        public bool IsDeleted { get; set; }

        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; } = null!;

        public DateTime? UpdateDate { get; set; }
        public string? UpdatedBy { get; set; }
    }

    public class EntityMetadata
    {
        public const int UserMaxLength = 128;
    }

}
