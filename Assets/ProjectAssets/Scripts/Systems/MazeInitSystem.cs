using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Events;
using Project.Infrastructure;

namespace Project.Systems
{
    internal sealed class MazeInitSystem : IEcsInitSystem
    {
        [EcsShared] private readonly SharedData _data = default;
        private readonly EcsWorld _world = default;
        
        public void Init(EcsSystems systems)
        {
            var entity = _world.NewEntity();
            var pool = _world.GetPool<GenerateGridRequest>();
            ref var request = ref pool.Add(entity);
            request.Level = _data.Configuration.Levels[0];
        }
    }
}