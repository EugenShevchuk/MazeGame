using System;
using System.Collections.Generic;
using Leopotam.EcsLite;

namespace Project.Extensions
{
    public static class FilterExtensions
    {
        public static T[] GetArray<T>(this EcsFilter filter) where T : struct
        {
            var pool = filter.GetWorld().GetPool<T>();
            var arr = new T[filter.GetEntitiesCount()];
            var iterator = filter.GetEnumerator();
            iterator.MoveNext();
            for (int i = 0; i < filter.GetEntitiesCount(); i++)
            {
                arr[i] = pool.Get(iterator.Current);
                iterator.MoveNext();
            }
            iterator.Dispose();
            return arr;
        }

        public static List<T> GetList<T>(this EcsFilter filter, EcsPool<T> pool = null) where T : struct
        {
            pool ??= filter.GetWorld().GetPool<T>();

            var list = new List<T>();
            
            foreach (var i in filter)
                list.Add(pool.Get(i));

            return list;
        }

        public static int GetRandomEntity(this EcsFilter filter, Random random)
        {
            return filter.GetRawEntities()[random.Next(filter.GetEntitiesCount())];
        }
    }
}