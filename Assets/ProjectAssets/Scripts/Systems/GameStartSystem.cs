using System.Threading.Tasks;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Events;
using Project.Infrastructure;
using Project.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Project.Systems
{
    internal sealed class GameStartSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(StartGameEvent))]
        private readonly EcsFilter _startGameEvent = default;

        private readonly EcsPool<StartGameEvent> _startGamePool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _startGameEvent)
            {
                StartGameAsync();
                _startGamePool.Del(i);
            }
        }

        private void StartGameAsync()
        {
            Addressables.LoadSceneAsync(Constants.GAME);
            Addressables.LoadSceneAsync(Constants.IN_GAME_UI, LoadSceneMode.Additive);
        }
    }
}