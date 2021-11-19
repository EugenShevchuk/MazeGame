using System.Collections.Generic;
using Leopotam.EcsLite;
using Project.Components;
using Project.Utilities;

namespace Project.Systems
{
    internal sealed class DijkstraMazeBraidingSystem : IEcsRunSystem
    {

        private readonly EcsPool<Cell> _cellPool = default;
        
        public void Run(EcsSystems systems)
        {
            
        }

        private void SetCost(int startEntity, int endEntity)
        {
            ref var start = ref _cellPool.Get(startEntity);

            var visited = new HashSet<int>();

            var frontier = new MinHeap<int>();
        }
    }
}