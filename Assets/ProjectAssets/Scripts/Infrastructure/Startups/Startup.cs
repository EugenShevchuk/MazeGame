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
        private Actions _actions;
        
        private void Start()
        {
            _world = new EcsWorld();
            _data.CurrentWorld = _world;
            
            _systems = new EcsSystems(_world, _data);
            _actions = new Actions();
            _actions.Enable();
            
            _systems
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                .ConvertScene()
                
                .AddGroup("GridGeneration", false, null,
                    new GridGeneratingSystem(_world, _data),
                    new GridSetupSystem(_world, _data),
                    new CameraCentringSystem(_world, _data),
                    
                    new CellViewCreatingSystem(_world, _data),
                    new ProcessorViewCreatingSystem(_world, _data),
                    new ProcessorSurroundingViewCreatingSystem(_world, _data),
                    new SpawnerViewCreatingSystem(_world, _data))
                
                .AddGroup("MazeGeneration", false, null,
                    new MazeGeneratingSystem(_world, _data),
                    new MazeBraidingSystem(_world, _data),
                    
                    new CellViewUpdatingSystem(_world))
                
                .Add(new GameSceneInitSystem())
                
                .Add(new CameraInputSystem(_actions))
                .Add(new CameraMovementSystem())

                .Add(new ItemSelectingSystem())
                .Add(new SelectedItemManagingSystem())
                .Add(new UIUpdatingSystem())
                
                .Add(new BugCreatingSystem())
                
                .Add(new BugPathfindingSystem())
                .Add(new BugMoveSystem())

                .Add(new BugViewCreatingSystem(_world, _data))
                .Add(new PathVisualizingSystem())
                
                .DelHere<SelectRequest>()
                .DelHere<LeftMouseButtonPressedEvent>()
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
                _actions.Disable();
                _actions.Dispose();
                _systems.Destroy();
                _systems = null;
                _world.Destroy();
                _world = null;
            }
        }
    }
}