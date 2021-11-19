using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Extensions;
using Project.UnityComponents;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class BugMoveSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsFilter(typeof(PlayerEndedTurnEvent))]
        private readonly EcsFilter _endTurnEvent = default;

        [EcsFilter(typeof(Bug), typeof(WorldObject), typeof(ObjectViewRef), typeof(HasPath))]
        [EcsFilterExclude(typeof(EndedTurn))]
        private readonly EcsFilter _bugs = default;

        private readonly EcsPool<HasPath> _pathPool = default;
        private readonly EcsPool<WorldObject> _worldObjectPool = default;
        private readonly EcsPool<EndedTurn> _endedTurnPool = default;
        private readonly EcsPool<ObjectViewRef> _viewRefPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var turn in _endTurnEvent)
            {
                var bugsToMove = new Dictionary<int, BugView>();
                
                foreach (var i in _bugs)
                {
                    ref var path = ref _pathPool.Get(i);
                    var bugView = (BugView)_viewRefPool.Get(i).View;
                    
                    bugsToMove.Add(i, bugView);
                }
                
                MoveAllBugs(bugsToMove).Forget();
            }
        }

        private async UniTask MoveAllBugs(Dictionary<int, BugView> bugsToMove)
        {
            foreach (var pair in bugsToMove)
            {
                var path = _pathPool.Get(pair.Key);
                await pair.Value.Move(GetPath(path.Path, 5), 1f);
                _endedTurnPool.Add(pair.Key);
            }
            
            _world.SendMessage(new AIEndedTurnEvent());
        }

        private List<Vector3> GetPath(List<EcsPackedEntityWithWorld> entities, int moveDistance)
        {
            var path = new List<Vector3>();

            for (int i = 0; i < moveDistance; i++)
            {
                if (entities[0].Unpack(out var w, out var entity))
                {
                    var position = _worldObjectPool.Get(entity).Transform.position;
                    path.Add(position);
                    entities.Remove(entities[0]);
                }
            }

            return path;
        }
    }
}