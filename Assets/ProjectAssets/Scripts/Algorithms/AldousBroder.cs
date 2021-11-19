using System;
using System.Linq;
using Project.Components;
using Project.Infrastructure;

namespace Project.Model.Algorithms
{
    public sealed class AldousBroder : IMazeAlgorithm
    {
        public void GenerateMaze(Cell[] mazeCells, Cell[] borderCells = null, Cell[] surroundingCells = null, Level level = default)
        {
            var random = new Random();
           
            var cell = mazeCells[random.Next(mazeCells.Length)];
            var unvisited = mazeCells.Length - 1;

            while (unvisited > 0)
            {
                var index = random.Next(cell.Neighbors.Count);
                var neighbor = cell.Neighbors.ElementAt(index);
/*
                if (neighbor..Count == 0)
                {
                    cell.Link(neighbor.PackedEntity);
                    unvisited -= 1;
                }
                
                cell = neighbor;*/
            }
        }
    }
}