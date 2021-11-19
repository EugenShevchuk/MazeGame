using Project.Infrastructure;
using Project.Model.Algorithms;

namespace Project.Events
{
    public struct GenerateMazeRequest
    {
        public IMazeAlgorithm Algorithm;
    }
}