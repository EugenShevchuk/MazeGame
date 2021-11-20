using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Extensions;
using Project.Infrastructure;
using Project.Utilities;

namespace Project.Systems
{
    internal sealed class UIUpdatingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsShared] private readonly SharedData _data = default;
        
        [EcsFilter(typeof(SelectRequest))] 
        private readonly EcsFilter _request = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _request)
            {
                if (i.Has<Processor>(_world))
                    _data.SelectedUI.UpdateSelected(SelectableType.Processor);
                else if (i.Has<Bug>(_world))
                    _data.SelectedUI.UpdateSelected(SelectableType.Bug);
                
                
            }
        }
    }
}