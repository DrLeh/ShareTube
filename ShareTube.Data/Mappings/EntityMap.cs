using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShareTube.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ShareTube.Data.Mappings
{
    internal abstract class EntityMap<T> : EntityMap
        where T : Entity
    {
        public abstract void Map(EntityTypeBuilder<T> e);
    }

    internal class EntityMap
    {

    }

    internal static class MapFactory
    {
        private static Dictionary<Type, Type> _typeMap;
        private static Dictionary<Type, Type> TypeMap => _typeMap ?? (_typeMap = BuildMap());

        private static Dictionary<Type, Type> BuildMap()
        {
            var types =  Assembly.GetAssembly(typeof(EntityMap<>))
                .GetTypes()
                .Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() ==  typeof(EntityMap<>))
                .ToDictionary(x => x.BaseType.GenericTypeArguments.First());

            return types;
        }

        private static IEnumerable<Type> TypesThatInheritFrom<T>()
        {
            return Assembly.GetAssembly(typeof(T))
                .GetTypes()
                .Where(x => typeof(T).IsAssignableFrom(x) && !x.IsInterface && x.GetGenericArguments().Any());
            //.Where(t => t.IsAssignableFrom(typeof(T)) && !t.IsInterface && t.GetGenericArguments().Any());
        }

        public static EntityMap<T> Create<T>()
            where T : Entity
        {
            if (TypeMap.ContainsKey(typeof(T)))
            {
                return (EntityMap<T>)Activator.CreateInstance(TypeMap[typeof(T)]);
            }
            return null;
        }
    }
}
