using System.Collections.Generic;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Extensions;
using Project.Infrastructure;
using UnityEngine;
using Grid = Project.Infrastructure.Grid;

namespace Project.Systems
{
    internal sealed class GridGeneratingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly SharedData _data;

        private readonly EcsPool<Cell> _cellPool;
        private readonly EcsPool<CreateViewRequest> _viewRequestPool;

        internal GridGeneratingSystem(EcsWorld world, SharedData data)
        {
            _world = world;
            _data = data;
            _cellPool = world.GetPool<Cell>();
            _viewRequestPool = world.GetPool<CreateViewRequest>();
        }

        public void Run(EcsSystems systems)
        {
            CreateGrid(_data.CurrentLevel);
        }

        private void CreateGrid(in Level level)
        {
            _data.Grid = new Grid
            {
                Size = level.MazeSize * level.MazeSize,
                SurroundingCells = new Cell[8],
                Cells = new Cell[level.MazeSize][]
            };

            for (int i = 0; i < level.MazeSize; i++)
            {
                _data.Grid.Cells[i] = new Cell[level.MazeSize];

                for (int j = 0; j < level.MazeSize; j++)
                {
                    var entity = _world.NewEntity();
                    _data.Grid.Cells[i][j] = CreateCellEntity(i, j, entity);

                    if (i == level.MazeSize / 2 && j == level.MazeSize / 2)
                        entity.Add<Processor>(_world);
                    
                    else if (CheckForSurroundings(i, j, level.MazeSize / 2))
                        entity.Add<ProcessorSurroundings>(_world);
                    
                    AssignBorderComponents(i, j, entity, level.MazeSize);
                }
            }
        }

        private Cell CreateCellEntity(int x, int y, int entity)
        {
            ref var cell = ref _cellPool.Add(entity);
            ref var request = ref _viewRequestPool.Add(entity);

            request.SpawnPosition = new Vector3(x, y, 0);
            
            cell.Links = new List<EcsPackedEntityWithWorld>();
            cell.Neighbors = new HashSet<EcsPackedEntityWithWorld>();
            cell.CellPool = _cellPool;
            cell.Entity = _world.PackEntityWithWorld(entity);
            cell.Position = new Vector2Int(x, y);

            return cell;
        }

        private bool CheckForSurroundings(int x, int y, int center)
        {
            return x == center - 1 && y == center ||
                   x == center + 1 && y == center ||
                   x == center && y == center - 1 ||
                   x == center && y == center + 1 ||
                   x == center - 1 && y == center - 1 ||
                   x == center + 1 && y == center + 1 ||
                   x == center - 1 && y == center + 1 ||
                   x == center + 1 && y == center - 1;
        }

        private void AssignBorderComponents(int x, int y, int entity, int size)
        {
            if (x == size - 1)
                entity.Add<OnEastBorder>(_world);

            if (x == 0)
                entity.Add<OnWestBorder>(_world);

            if (y == size - 1)
                entity.Add<OnNorthBorder>(_world);

            if (y == 0)
                entity.Add<OnSouthBorder>(_world);
        }
    }
}