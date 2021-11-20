using Leopotam.EcsLite;

namespace Project.Extensions
{
    public static class EntityExtensions
    {
        public static ref T Get<T>(this int entity, EcsWorld world) where T : struct
        {
            return ref world.GetPool<T>().Get(entity);
        }

        public static ref T Add<T>(this int entity, EcsWorld world) where T : struct
        {
            return ref world.GetPool<T>().Add(entity);
        }

        public static bool Has<T>(this int entity, EcsWorld world, EcsPool<T> pool = null) where T : struct
        {
            pool ??= world.GetPool<T>();
            return pool.Has(entity);
        }
        
        public static bool Has<T>(this int entity, EcsPool<T> pool) where T : struct
        {
            return pool.Has(entity);
        }

        public static void Del<T>(this int entity, EcsWorld world, EcsPool<T> pool = null) where T : struct
        {
            pool ??= world.GetPool<T>();
            pool.Del(entity);
        }
        
        public static void Del<T>(this int entity, EcsPool<T> pool) where T : struct
        {
            pool.Del(entity);
        }
    }
}