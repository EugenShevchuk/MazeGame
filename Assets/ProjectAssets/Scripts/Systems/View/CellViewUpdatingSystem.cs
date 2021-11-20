using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.UnityComponents;
using Project.Utilities;

namespace Project.Systems
{
    internal sealed class CellViewUpdatingSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(CreateViewRequest), typeof(Cell), typeof(ObjectViewRef))]
        [EcsFilterExclude(typeof(Processor), typeof(ProcessorSurroundings))]
        private readonly EcsFilter _cellToUpdate = default;

        private readonly EcsPool<Cell> _cellPool = default;
        private readonly EcsPool<ObjectViewRef> _cellViewPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _cellToUpdate)
            {
                ref var cell = ref _cellPool.Get(i);
                var view = (CellView) _cellViewPool.Get(i).View;
                
                SetupView(view, WhereToPlaceWalls(ref cell));
            }
        }
        
        private void SetupView(CellView view, List<Direction> directions)
        {
            var left = true;
            var right = true;
            var top = true;
            var bottom = true;
            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case Direction.South:
                        bottom = false;
                        break;
                    case Direction.North:
                        top = false;
                        break;
                    case Direction.West:
                        left = false;
                        break;
                    case Direction.East:
                        right = false;
                        break;
                }
            }
            
            view.SetupView(left, right, bottom, top);
        }

        private List<Direction> WhereToPlaceWalls(ref Cell cell)
        {
            var dirs = new List<Direction>(4);

            foreach (var entity in cell.Neighbors)
                if (cell.Links.Contains(entity))
                {
                    if (cell.Entity.Unpack(out var world, out var i))
                    {
                        var x = cell.Position.x;
                        var y = cell.Position.y;
                        
                        if (entity.Unpack(out var w, out var j))
                        {
                            ref var linkedCell = ref _cellPool.Get(j);

                            var linkedX = linkedCell.Position.x;
                            var linkedY = linkedCell.Position.y;

                            if (linkedX == x && linkedY == y - 1)
                            {
                                dirs.Add(Direction.South);
                            }
                            if (linkedX == x && linkedY == y + 1)
                            {
                                dirs.Add(Direction.North);
                            }
                            if (linkedX == x - 1 && linkedY == y)
                            {
                                dirs.Add(Direction.West);
                            }
                            if (linkedX == x + 1 && linkedY == y)
                            {
                                dirs.Add(Direction.East);
                            }
                        }
                    }
                }

            return dirs;
        }
    }
}