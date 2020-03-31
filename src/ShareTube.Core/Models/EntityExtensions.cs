using ShareTube.Core.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShareTube.Core.Models
{
    public static class EntityExtensions
    {
        public static T ValidateFound<T>(this T obj, long id)
            where T : IEntity
        {
            if (obj == null)
                throw new EntityNotFoundException(id, typeof(T).Name);
            return obj;
        }

        public static T ValidateFound<T>(this T obj, string key, string keyName)
            where T : IEntity
        {
            if (obj == null)
                throw new EntityNotFoundException(key, keyName, typeof(T).Name);
            return obj;
        }
    }
}
