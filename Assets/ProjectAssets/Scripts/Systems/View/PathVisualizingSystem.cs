using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Utilities;

namespace Project.Systems
{
    internal sealed class PathVisualizingSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(Bug), typeof(HasPath), typeof(VisualizePathRequest))]
        private readonly EcsFilter _bugs = default;
        
        private readonly EcsPool<HasPath> _pathPool = default;
        private readonly EcsPool<ObjectViewRef> _viewRefPool = default;
        private readonly EcsPool<VisualizePathRequest> _visualizePathRequestPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _bugs)
            {
                ref var path = ref _pathPool.Get(i);
                var color = Utils.Colors[0];

                foreach (var packedEntity in path.Path)
                {
                    if (packedEntity.Unpack(out var world, out var entity))
                    {
                        var view = _viewRefPool.Get(entity).View;
                        view.Renderer.color = color;
                    }
                }

                _visualizePathRequestPool.Del(i);
            }
        }
    }
}