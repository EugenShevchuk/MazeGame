using System;
using System.Linq;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Extensions;
using Project.Infrastructure;
using UnityEngine;
using Random = System.Random;

namespace Project.Systems
{
    internal sealed class MazeBraidingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly SharedData _data;

        private readonly EcsFilter _deadEnds = default;

        private readonly EcsFilter _spawner = default;

        private readonly EcsPool<Cell> _cellPool = default;
        private readonly EcsPool<Spawner> _spawnerPool = default;
        private readonly EcsPool<DeadEnd> _deadEndPool = default;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool = default;

        public MazeBraidingSystem(EcsWorld world, SharedData data)
        {
            _world = world;
            _data = data;

            _deadEnds = world.Filter<Cell>().Inc<DeadEnd>().End();
            _spawner = world.Filter<Spawner>().End();
            _cellPool = world.GetPool<Cell>();
            _spawnerPool = world.GetPool<Spawner>();
            _deadEndPool = world.GetPool<DeadEnd>();
            _viewRequestPool = world.GetPool<CreateViewRequest>();
        }

        public void Run(EcsSystems systems)
        {
            var random = new Random((int) DateTime.Now.Ticks);
                
            //Braid(random);
                
            RequestCellViewUpdate();
            RequestSpawnerViewCreation();
            _world.SendMessage(new MazeIsReady());
        }
        
        private void Braid(Random random)
        {
            foreach (var i in _deadEnds)
            {
                if (_data.CurrentLevel.BraidChance > (float)random.NextDouble())
                    continue;
                
                ref var cell = ref _cellPool.Get(i);

                var unlikedNeighbors = cell.Neighbors;
                
                foreach (var link in cell.Links)
                    unlikedNeighbors.Remove(link);

                var packedEntity = unlikedNeighbors.ElementAt(random.Next(unlikedNeighbors.Count));
                
                cell.Link(packedEntity);

                _deadEndPool.Del(i);
            }
        }
        
        private void RequestCellViewUpdate()
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