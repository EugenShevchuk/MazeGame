using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Extensions;

namespace Project.Systems
{
    internal sealed class SelectedItemManagingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsFilter(typeof(SelectRequest))] 
        private readonly EcsFilter _request = default;

        [EcsFilter(typeof(Selected))] 
        private readonly EcsFilter _selected = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _request)
            {
                if (_selected.GetEntitiesCount() > 0)
                    foreach (var entity in _selected.GetRawEntities())
                        entity.Del<Selected>(_world);

                i.Add<Selected>(_world);
            }
        }
    }
}