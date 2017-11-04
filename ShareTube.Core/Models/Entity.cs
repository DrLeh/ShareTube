using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Models
{
    //public abstract class BaseEntity<TKey>
    //{
    //    public abstract TKey Id { get; set; }
    //}

    //public class Entity : BaseEntity<long>
    //{
    //    public override long Id { get; set; }
    //}

    public class  Entity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
