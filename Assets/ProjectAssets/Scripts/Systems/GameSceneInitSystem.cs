using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.ExtendedSystems;
using Project.Events;
using Project.Infrastructure;

namespace Project.Systems
{
    internal sealed class GameSceneInitSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsShared] private readonly SharedData _data = default;

        [EcsFilter(typeof(GridIsReady))] 
        private readonly EcsFilter _gridIsReady = default;

        [EcsFilter(typeof(GenerateMazeRequest))]
        private readonly EcsFilter _generateMazeRequest = default;

        [EcsFilter(typeof(MazeIsReady))] 
        private readonly EcsFilter _mazeIsReady = default;

        private readonly EcsPool<GridIsReady> _gridIsReadyPool = default;
        private readonly EcsPool<MazeIsReady> _mazeIsReadyPool = default;
        private readonly EcsPool<GenerateMazeRequest> _generateMazeRequestPool = default;

        public void Init(EcsSystems systems)
        {
            _data.CurrentLevel = _data.Configuration.Levels[0];
            GridGenerationSetActive(true);
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _gridIsReady)
            {
                GridGenerationSetActive(false);
                MazeGenerationSetActive(true);
                _gridIsReadyPool.Del(i);
            }
            
            foreach (var i in _generateMazeRequest)
            {
                MazeGenerationSetActive(true);
                _generateMazeRequestPool.Del(i);
            }

            foreach (var i in _mazeIsReady)
            {
                MazeGenerationSetActive(false);
                _mazeIsReadyPool.Del(i);
            }
        }

        private void GridGenerationSetActive(bool state)
        {
            var entity = _world.NewEntity();
            ref var evt = ref _world.GetPool<EcsGroupSystemState>().Add(entity);
            evt.Name = "GridGeneration";
            evt.State = state;
        }

        private void MazeGenerationSetActive(bool state)
        {
            var entity = _world.NewEntity();
            ref var evt = ref _world.GetPool<EcsGroupSystemState>().Add(entity);
            evt.Name = "MazeGeneration";
            evt.State = state;
        }
    }
}