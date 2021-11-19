using Project.Components;
using Project.Infrastructure;

namespace Project.Model.Algorithms
{
    public interface IMazeAlgorithm
    {
        public void GenerateMaze(Cell[] mazeCells, Cell[] spawnerCells = null, Cell[] surroundingCells = null, Level level = default);
    }
}