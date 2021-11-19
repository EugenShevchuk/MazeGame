using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Leopotam.EcsLite.UnityEditor;
using Project.Events;
using Project.Systems;
using Reflex.Scripts.Attributes;
using UnityEngine;
using Voody.UniLeo.Lite;

namespace Project.Infrastructure
{
    internal sealed class Startup : MonoBehaviour
    {
        [MonoInject] private readonly SharedData _data = default;

        private EcsWorld _world;
        private EcsSystems _systems;
        
        private void Start()
        {
            _world = new EcsWorld();
            _data.CurrentWorld = _world;
            _systems = new EcsSystems(_data.CurrentWorld, _data);
            
            _systems
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .ConvertScene()
                
                .Add(new GameSceneInitSystem())
                .Add(new GridGeneratingSystem())
                .DelHere<GenerateGridRequest>()
                
                .Add(new GridSetupSystem())
                .DelHere<SetupGridRequest>()
                
                .Add(new MazeGeneratingSystem())
                .DelHere<GenerateMazeRequest>()
                
                .Add(new MazeBraidingSystem())
                
                .Add(new InputInitSystem())
                .Add(new CameraSystem())
                
                .Add(new BugCreatingSystem())
                
                .Add(new BugPathfindingSystem())
                .Add(new BugMoveSystem())
                
                .Add(new CellViewCreatingSystem())
                .Add(new CellViewUpdatingSystem())
                
                .Add(new ProcessorViewCreatingSystem())
                .Add(new ProcessorSurroundingViewCreatingSystem())
                .Add(new SpawnerViewCreatingSystem())
                
                .Add(new BugViewCreatingSystem())
                
                .Add(new PathVisualizingSystem())
                
                .DelHere<PlayerEndedTurnEvent>()
                .DelHere<CreateViewRequest>()
                
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