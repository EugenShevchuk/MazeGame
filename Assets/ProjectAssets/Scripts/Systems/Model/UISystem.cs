using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Extensions;

namespace Project.Systems
{
    internal sealed class UISystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsFilter(typeof(Selected))] 
        private readonly EcsFilter _selected = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _selected)
            {
                if (i.Has<Processor>(_world))
                {
                    
                }
                else if (i.Has<Bug>(_world))
                {
                    
                }
            }
        }
    }
}