using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Systems;
using Project.UI;
using Reflex.Scripts.Attributes;
using UnityEngine;

namespace Project.Infrastructure
{
    internal sealed class MainMenuStartup : MonoBehaviour
    {
        [MonoInject] private readonly SharedData _data = default;
        [SerializeField] private EcsWorldProvider _provider;
        private EcsWorld _world;
        private EcsSystems _systems;
        
        private void Start()
        {
            _world = new EcsWorld();
            _provider.SetWorld(_world);
            _systems = new EcsSystems(_world, _data);
            _systems
                .Add(new AssetsPreLoadingSystem())
                .Add(new GameStartSystem())
                
                .Inject()
                .Init();
        }

        private void Update()
        {
            _systems?.Run();
        }

        private void OnDestroy()
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}