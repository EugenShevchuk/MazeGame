using System;
using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Utilities;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class BugPathfindingSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(Cell))] 
        private readonly EcsFilter _allCells = default;
        
        [EcsFilter(typeof(Bug), typeof(ObjectViewRef))]
        [EcsFilterExclude(typeof(HasPath))]
        private readonly EcsFilter _bug = default;

        [EcsFilter(typeof(PlayerEndedTurnEvent))]
        private readonly EcsFilter _endTurnEvent = default;
        
        private readonly EcsPool<Bug> _bugPool = default;
        private readonly EcsPool<Cell> _cellPool = default;
        private readonly EcsPool<HasPath> _hasPathPool = default;
        private readonly EcsPool<WorldObject> _worldObjectPool = default;
        private readonly EcsPool<VisualizePathRequest> _visualizePathRequestPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var turn in _endTurnEvent)
            {
                foreach (var i in _bug)
                {
                    var bug = _bugPool.Get(i);

                    ref var path = ref _hasPathPool.Add(i);
                    
                    path.Path = GetPath(bug.CurrentCell, bug.Processor);

                    _visualizePathRequestPool.Add(i);
                }
            }
        }

        private List<EcsPackedEntityWithWorld> GetPath(Cell startEntity, EcsPackedEntityWithWorld endEntity)
        {
            foreach (var i in _allCells)
            {
                ref var cell = ref _cellPool.Get(i);
                cell.GCost = int.MaxValue;
            }
            
            if (endEntity.Unpack(out var world, out var entity))
            {
                ref var endCell = ref _cellPool.Get(entity);
                
                var visited = new HashSet<Cell>();
                visited.Add(startEntity);

                var frontier = new MinHeap<Cell>((lhs, rhs) => lhs.FCost.CompareTo(rhs.FCost));
                frontier.Add(startEntity);

                while (frontier.Count > 0)
                {
                    var current = frontier.Remove();

                    if (current.Entity.EqualsTo(endEntity))
                        break;

                    foreach (var neighbor in current.Neighbors)
                    {
                        if (neighbor.Unpack(out var w, out var i))
                        {
                            ref var neighborCell = ref _cellPool.Get(i);

                            int newNeighborGCost;
                            
                            if (current.Links.Contains(neighbor))
                                newNeighborGCost = current.GCost + 1;
                            else
                                newNeighborGCost = current.GCost + 50;
                            
                            if (newNeighborGCost < neighborCell.GCost)
                            {
                                neighborCell.GCost = newNeighborGCost;
                                neighborCell.HCost = GetDistance(neighborCell.Position, endCell.Position);

                                neighborCell.PreviousCell = current.Entity;
                            }

                            if (visited.Contains(neighborCell) == false)
                            {
                                visited.Add(neighborCell);
                                frontier.Add(neighborCell);
                            }
                        }
                    }
                }
                return BacktrackToPath(startEntity.Entity, endEntity);
            }
            throw new Exception("Cannot unpack target entity");
        }
        
        private List<EcsPackedEntityWithWorld> BacktrackToPath(EcsPackedEntityWithWorld start, EcsPackedEntityWithWorld end)
        {
            var current = end;
            var path = new List<EcsPackedEntityWithWorld>();

            while (end.EqualsTo(start) == false)
            {
                if (current.Unpack(out var w, out var i) && path.Contains(current) == false)
                {
                    path.Add(current);
                    current = _cellPool.Get(i).PreviousCell;
                }
                else
                    break;
            }
            
            path.Reverse();

            return path;
        }

        private int GetDistance(Vector2Int current, Vector2Int target)
        {
            var distance = target - current;
            return distance.x + distance.y;
        }
    }
}