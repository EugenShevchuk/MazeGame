using System;
using System.Collections.Generic;
using System.Linq;
using Project.Components;
using Project.Extensions;
using Project.Infrastructure;
using Random = System.Random;

namespace Project.Model.Algorithms
{
    public sealed class Wilson : IMazeAlgorithm
    {
        public void GenerateMaze(Cell[] mazeCells, Cell[] spawnerCells, Cell[] surroundingCells, Level level)
        {
            var random = new Random((int)DateTime.Now.Ticks);
            var spawners = 0;
            var unvisited = mazeCells.Concat(surroundingCells).ToList();
            var first = surroundingCells.GetRandomElement(random);
            unvisited.Remove(first);
            
            while (unvisited.Any())
            {
                Cell next;
                var walk = new List<Cell>();

                if (spawners < level.SpawnerCount)
                {
                    var pathCount = random.Next(1, 4);
                    next = spawnerCells.GetRandomElement(random);
                    for (int i = 0; i < pathCount; i++)
                    {
                        walk.Add(next);
                        spawners++;
                    
                        while (unvisited.Contains(next))
                        {
                            //next = next.NeighborsArray[random.Next(next.NeighborsArray.Length)];
                        
                            if (walk.IndexOf(next) >= 0) 
                            {
                                walk = walk.Take(walk.IndexOf(next) + 1).ToList();
                            }
                            else 
                            {
                                walk.Add(next);
                            }
                        }
                    }
                }
                else
                {
                    next = unvisited.GetRandomElement(random);
                    walk.Add(next);
                    
                    while (unvisited.Contains(next))
                    {
                        //next = next.NeighborsArray[random.Next(next.NeighborsArray.Length)];
                        if (walk.IndexOf(next) >= 0) 
                        {
                            walk = walk.Take(walk.IndexOf(next) + 1).ToList();
                        }
                        else 
                        {
                            walk.Add(next);
                        }
                    }
                }
                

                //walk.Zip(walk.Skip(1), (thisCell, nextCell) => (thisCell, nextCell))
                //    .ForEach(c => c.thisCell.Link(c.nextCell));
                
                walk.ForEach(c => unvisited.Remove(c));
            }
        }
    }
}