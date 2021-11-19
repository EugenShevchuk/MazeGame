using System;
using System.Collections.Generic;
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
    internal sealed class GridSetupSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = default;
        
        [EcsShared] private readonly SharedData _data = default;

        [EcsFilter(typeof(SetupGridRequest))] 
        private readonly EcsFilter _request = default;
        private readonly EcsPool<SetupGridRequest> _requestPool = default;

        [EcsFilter(typeof(Cell))] 
        private readonly EcsFilter _allCells = default;

        [EcsFilter(typeof(Processor), typeof(WorldObject))] 
        private readonly EcsFilter _processor = default;

        [EcsFilter(typeof(ProcessorSurroundings))]
        private readonly EcsFilter _processorSurroundings = default;
        
        [EcsFilter(typeof(Cell), typeof(OnEastBorder))]
        [EcsFilterExclude(typeof(OnNorthBorder), typeof(OnSouthBorder))]
        private readonly EcsFilter _eastBorderCells = default;

        [EcsFilter(typeof(Cell), typeof(OnWestBorder))]
        [EcsFilterExclude(typeof(OnNorthBorder), typeof(OnSouthBorder))]
        private readonly EcsFilter _westBorderCells = default;

        [EcsFilter(typeof(Cell), typeof(OnNorthBorder))]
        [EcsFilterExclude(typeof(OnEastBorder), typeof(OnWestBorder))]
        private readonly EcsFilter _northBorderCells = default;

        [EcsFilter(typeof(Cell), typeof(OnSouthBorder))]
        [EcsFilterExclude(typeof(OnEastBorder), typeof(OnWestBorder))]
        private readonly EcsFilter _southBorderCells = default;

        private readonly EcsPool<Cell> _cellPool = default;

        public void Run(EcsSystems systems)
        {
            foreach (var i in _request)
            {
                ref var request = ref _requestPool.Get(i);

                SetCellNeighbors(request.Level.MazeSize);
                LinkProcessorArea();
                SelectSpawnerCells();
                
                _requestPool.Del(i);
            }
        }

        private void SetCellNeighbors(int mazeSize)
        {
            foreach (var i in _allCells)
            {
                ref var cell = ref _cellPool.Get(i);

                var row = cell.Position.x;
                var col = cell.Position.y;

                if (row - 1 >= 0)
                    cell.Neighbors.Add(_data.Grid.Cells[row - 1][col].Entity);
                if (row + 1 < mazeSize)
                    cell.Neighbors.Add(_data.Grid.Cells[row + 1][col].Entity);
                if (col - 1 >= 0)
                    cell.Neighbors.Add(_data.Grid.Cells[row][col - 1].Entity);
                if (col + 1 < mazeSize)
                    cell.Neighbors.Add(_data.Grid.Cells[row][col + 1].Entity);
            }
        }

        private void LinkProcessorArea()
        {
            foreach (var i in _processor)
            {
                ref var processorCell = ref _cellPool.Get(i);
                
                var processorX = processorCell.Position.x;
                var processorY = processorCell.Position.y;

                foreach (var entity in _processorSurroundings)
                {
                    ref var surroundingCell = ref _cellPool.Get(entity);

                    var x = surroundingCell.Position.x;
                    var y = surroundingCell.Position.y;

                    if (surroundingCell.Neighbors.Contains(processorCell.Entity))
                    {
                        processorCell.Link(surroundingCell.Entity);
                        return;
                    }

                    if (x == processorX + 1 && y == processorY + 1)
                    {
                        surroundingCell.Link(_data.Grid.Cells[x][y - 1].Entity);
                        surroundingCell.Link(_data.Grid.Cells[x - 1][y].Entity);
                        return;
                    }

                    if (x == processorX - 1 && y == processorY + 1)
                    {
                        surroundingCell.Link(_data.Grid.Cells[x + 1][y].Entity);
                        surroundingCell.Link(_data.Grid.Cells[x][y - 1].Entity);
                        return;
                    }

                    if (x == processorX + 1 && y == processorY - 1)
                    {
                        surroundingCell.Link(_data.Grid.Cells[x][y + 1].Entity);
                        surroundingCell.Link(_data.Grid.Cells[x - 1][y].Entity);
                        return;
                    }

                    if (x == processorX - 1 && y == processorY - 1)
                    {
                        surroundingCell.Link(_data.Grid.Cells[x][y + 1].Entity);
                        surroundingCell.Link(_data.Grid.Cells[x + 1][y].Entity);
                        return;
                    }
                }
            }
        }
        
        // TODO: Make algorithm that will consider distance between spawners
        private Cell[] SelectSpawnerCells()
        {
            if (_data.CurrentLevel.SpawnerCount > 4)
                Debug.LogError("There is too many spawners");

            var spawners = new Cell[_data.CurrentLevel.SpawnerCount];
            var random = new Random((int)DateTime.Now.Ticks);
            
            var sides = new List<EcsFilter>
            {
                _eastBorderCells,
                _westBorderCells,
                _northBorderCells,
                _southBorderCells
            };

            for (int i = 0; i < _data.CurrentLevel.SpawnerCount; i++)
            {
                var side = sides[random.Next(sides.Count)];
                var cellEntity = side.GetRandomEntity(random);
                spawners[i] = cellEntity.Get<Cell>(_world);
                
                CreateSpawner(cellEntity, spawners[i]);
                
                sides.Remove(side);
            }
            
            return spawners;
        }
        
        private void CreateSpawner(int cellEntity, Cell parent)
        {
            var spawnerEntity = _world.NewEntity();
            ref var spawner = ref _world.GetPool<Spawner>().Add(spawnerEntity);
            
            spawner.Parent = parent;
            spawner.TurnsToSpawn = 1;
            
            var occupiedPool = _world.GetPool<Occupied>();
            ref var occupied = ref occupiedPool.Add(cellEntity);
            occupied.Occupier = spawnerEntity;
        }
    }
}