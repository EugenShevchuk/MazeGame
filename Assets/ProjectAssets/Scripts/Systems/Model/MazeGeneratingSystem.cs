using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;
using Random = System.Random;

namespace Project.Systems
{
    internal sealed class MazeGeneratingSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private readonly SharedData _data;
        
        private readonly EcsFilter _mazeCells;
        
        private readonly EcsFilter _spawners;

        private readonly EcsPool<Cell> _cellPool = default;
        private readonly EcsPool<DeadEnd> _deadEndPool = default;
        private readonly EcsPool<Spawner> _spawnerPool = default;

        public MazeGeneratingSystem(EcsWorld world, SharedData data)
        {
            _data = data;
            _world = world;

            _mazeCells = world.Filter<Cell>().Exc<Processor>().Exc<ProcessorSurroundings>().End();
            _spawners = world.Filter<Spawner>().End();

            _cellPool = world.GetPool<Cell>();
            _deadEndPool = world.GetPool<DeadEnd>();
            _spawnerPool = world.GetPool<Spawner>();
        }

        public void Run(EcsSystems systems)
        {
            if (_data.Grid.IsGenerated)
                SetMazeToDefault();

            GenerateMaze();
            //MarkDeadEnds();

            _data.Grid.IsGenerated = true;
        }

        private void GenerateMaze()
        {
            var unvisited = _mazeCells.GetList<Cell>();
            var random = new Random((int)DateTime.Now.Ticks);
            var walk = new List<Cell>();

            CreatePathsToProcessor(unvisited, walk, random);

            while (unvisited.Any())
            {
                var next = unvisited.GetRandomElement(random);
                walk.Add(next);
                Walk(unvisited, walk, next, random);
                walk.Clear();
            }
        }

        private void CreatePathsToProcessor(List<Cell> unvisited, List<Cell> walk, Random random)
        {
            foreach (var entity in _spawners)
            {
                //if (unvisited.Any())
                //    continue;
                
                var pathCount = random.Next(2, 4);
                ref var spawner = ref _spawnerPool.Get(entity);
                spawner.Path = new List<Cell>[pathCount];
                var next = spawner.Parent;
                walk.Add(next);

                Walk(unvisited, walk, next, random);
                walk.Clear();
            }
        }

        private void Walk(List<Cell> unvisited, List<Cell> walk, Cell next, Random random, bool remove = true)
        {
            while (unvisited.Contains(next))
            {
                var nextEntity = next.Neighbors.ElementAt(random.Next(next.Neighbors.Count));
                
                if (nextEntity.Unpack(out var w, out var i))
                    next = _cellPool.Get(i);

                if (walk.IndexOf(next) >= 0)
                    walk = walk.Take(walk.IndexOf(next) + 1).ToList();
                else
                    walk.Add(next);
            }

            for (int i = 1; i < walk.Count; i++)
                walk[i - 1].Link(walk[i].Entity);
            
            if (remove)
                foreach (var cell in walk)
                    unvisited.Remove(cell);
        }

        private void MarkDeadEnds()
        {
            foreach (var i in _mazeCells)
            {
                ref var cell = ref _cellPool.Get(i);
                
                if (cell.Neighbors.Count == 4 && cell.Links.Count < 2)
                    _deadEndPool.Add(i);
            }
        }

        private void SetMazeToDefault()
        {
            foreach (var entity in _mazeCells)
            {
                var cell = _cellPool.Get(entity);

                for (int i = cell.Links.Count - 1; i >= 0; i--)
                {
                    cell.UnLink(cell.Links[i]);
                }
            }
        }
    }
}