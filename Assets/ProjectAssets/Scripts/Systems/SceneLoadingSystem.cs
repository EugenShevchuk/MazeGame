using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Events;
using Project.Infrastructure;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Project.Systems
{
    internal sealed class SceneLoadingSystem : IEcsRunSystem
    {
        [EcsShared] private readonly SharedData _data = default;
        
        [EcsFilter(typeof(LoadSceneRequest))]
        private readonly EcsFilter _loadRequest = default;

        [EcsFilter(typeof(UnloadSceneRequest))]
        private readonly EcsFilter _unloadRequest = default;

        private readonly EcsPool<LoadSceneRequest> _loadRequestsPool = default;
        private readonly EcsPool<UnloadSceneRequest> _unloadRequestsPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _loadRequest)
            {
                ref var request = ref _loadRequestsPool.Get(i);
                LoadSceneAsync(request.Name, request.Mode);
                _loadRequestsPool.Del(i);
            }

            foreach (var i in _unloadRequest)
            {
                ref var request = ref _unloadRequestsPool.Get(i);
                UnLoadSceneAsync(request.Scene, request.Mode);
                _unloadRequestsPool.Del(i);
            }
        }

        private async void LoadSceneAsync(string name, LoadSceneMode mode)
        {
            var task = Addressables.LoadSceneAsync(name, mode).Task;
            
            await task;
            
            _data.ActiveScenes.Add(task.Result);
        }

        private async void UnLoadSceneAsync(SceneInstance scene, LoadSceneMode mode)
        {
            var task = Addressables.UnloadSceneAsync(scene).Task;

            await task;

            _data.ActiveScenes.Remove(task.Result);
        }
    }
}