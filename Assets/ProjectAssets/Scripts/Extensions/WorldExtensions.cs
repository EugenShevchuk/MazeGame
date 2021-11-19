using Leopotam.EcsLite;
using Project.Events;
using Project.Model.Algorithms;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Project.Extensions
{
    public static class WorldExtensions
    {
        public static void SendMessage<T>(this EcsWorld world, T message) where T : struct
        {
            var pool = world.GetPool<T>();
            var entity = world.NewEntity();
            pool.Add(entity);
        }

        public static void CreateLoadSceneRequest(this EcsWorld world, string sceneName, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var pool = world.GetPool<LoadSceneRequest>();
            var entity = world.NewEntity();
            ref var request = ref pool.Add(entity);
            request.Name = sceneName;
            request.Mode = mode;
        }

        public static void CreateGenerateMazeRequest(this EcsWorld world, IMazeAlgorithm algorithm)
        {
            var pool = world.GetPool<GenerateMazeRequest>();
            var entity = world.NewEntity();
            ref var request = ref pool.Add(entity);
            request.Algorithm = algorithm;
        }

        public static void CreateUnloadSceneRequest(this EcsWorld world, SceneInstance scene, LoadSceneMode mode = LoadSceneMode.Additive)
        {
            var pool = world.GetPool<UnloadSceneRequest>();
            var entity = world.NewEntity();
            ref var request = ref pool.Add(entity);
            request.Scene = scene;
            request.Mode = mode;
        }
    }
}