using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShareTube.Core.Security
{
    public interface ISecurityPolicy
    {
        IQueryable<T> Filter<T>(IQueryable<T> query) where T : Entity;
    }

    public class PermissiveSecurityPolicy : ISecurityPolicy
    {
        public IQueryable<T> Filter<T>(IQueryable<T> query)
            where T : Entity
        {
            return query;
        }
    }
}
