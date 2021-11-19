using System;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Infrastructure;
using UnityEngine;
using Random = System.Random;

namespace Project.Systems
{
    internal sealed class BugCreatingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsShared] private readonly SharedData _data = default;
        
        [EcsFilter(typeof(PlayerEndedTurnEvent))] 
        private readonly EcsFilter _endTurnEvent = default;

        [EcsFilter(typeof(Processor))] 
        private readonly EcsFilter _processor = default;

        [EcsFilter(typeof(Spawner))]
        private readonly EcsFilter _spawners = default;

        private readonly EcsPool<Bug> _bugPool = default;
        private readonly EcsPool<WorldObject> _worldObjectPool = default;
        private readonly EcsPool<Spawner> _spawnerPool = default;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var turn in _endTurnEvent)
            {
                foreach (var i in _spawners)
                {
                    ref var spawner = ref _spawnerPool.Get(i);

                    if (spawner.TurnsToSpawn == 0)
                    {
                        ref var spawnerWorldObject = ref _worldObjectPool.Get(i);

                        CreateBug(spawner.Parent, spawnerWorldObject.Transform);
                        
                        var random = new Random((int) DateTime.Now.Ticks);
                        var turnRange = _data.CurrentLevel.TurnsBetweenSpawns;
                        spawner.TurnsToSpawn = random.Next(turnRange.x, turnRange.y);
                    }
                    else
                        spawner.TurnsToSpawn--;
                }
            }
        }

        private void CreateBug(Cell parent, Transform parentTransform)
        {
            var entity = _world.NewEntity();
            ref var bug = ref _bugPool.Add(entity);
            
            bug.CurrentCell = parent;
            bug.Type = SelectBugType();
            bug.ThisEntity = _world.PackEntityWithWorld(entity);

            foreach (var i in _processor)
                bug.Processor = _world.PackEntityWithWorld(i);

            ref var request = ref _viewRequestPool.Add(entity);
            request.SpawnPosition = parentTransform.position;
        }

        private BugType SelectBugType()
        {
            return BugType.Base;
        }
    }
}