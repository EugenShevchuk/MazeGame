using System;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Infrastructure;
using UnityEngine;
using Random = System.Random;

namespace Project.Systems
{
    internal sealed class MazeBraidingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;

        [EcsShared] private readonly SharedData _data = default;

        [EcsFilter(typeof(BraidMazeRequest))] 
        private readonly EcsFilter _request = default;

        [EcsFilter(typeof(Cell), typeof(DeadEnd))]
        private readonly EcsFilter _deadEnds = default;

        [EcsFilter(typeof(Spawner))] 
        private readonly EcsFilter _spawner = default;

        private readonly EcsPool<Cell> _cellPool = default;
        private readonly EcsPool<Spawner> _spawnerPool = default;
        private readonly EcsPool<DeadEnd> _deadEndPool = default;
        private readonly EcsPool<BraidMazeRequest> _requestPool = default;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _request)
            {
                var random = new Random((int) DateTime.Now.Ticks);
                
                //Braid(random);
                
                RequestCellViewCreation();
                RequestSpawnerViewCreation();
                
                _requestPool.Del(i);
            }
        }
        
        private void Braid(Random random)
        {
            foreach (var i in _deadEnds)
            {
                //if (_data.CurrentLevel.BraidChance > (float)random.NextDouble())
                 //   continue;
                
                ref var cell = ref _cellPool.Get(i);

                var unlikedNeighbors = cell.Neighbors;
                
                foreach (var link in cell.Links)
                    unlikedNeighbors.Remove(link);

                var packedEntity = unlikedNeighbors.ElementAt(random.Next(unlikedNeighbors.Count));
                
                cell.Link(packedEntity);

                _deadEndPool.Del(i);
            }
        }
        
        private void RequestCellViewCreation()
        {
            var requestPool = _world.GetPool<CreateViewRequest>();
            var filter = _world.Filter<Cell>().Inc<ObjectViewRef>().End();
            
            foreach (var i in filter)
                requestPool.Add(i);
        }

        private void RequestSpawnerViewCreation()
        {
            foreach (var i in _spawner)
            {
                var parentCell = _spawnerPool.Get(i).Parent;

                if (parentCell.Entity.Unpack(out var world, out var entity))
                {
                    var position = _cellPool.Get(entity).Position;
                    ref var request = ref _viewRequestPool.Add(i);
                    request.SpawnPosition = new Vector3(position.x, position.y, 0);
                }
            }
        }
    }
}