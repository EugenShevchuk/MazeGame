using Project.Pooling;
using Project.UnityComponents;

namespace Project.Infrastructure
{
    public sealed class AssetsData
    {
        public IPoolCreator<CellView> CellPoolCreator;
        public CellView CellPrefab;

        public IPoolCreator<ProcessorView> ProcessorPoolCreator;
        public ProcessorView ProcessorPrefab;

        public IPoolCreator<ProcessorSurroundingView> ProcessorSurroundingPoolCreator;
        public ProcessorSurroundingView ProcessorSurroundingPrefab;

        public IPoolCreator<SpawnerView> SpawnerPoolCreator;
        public SpawnerView SpawnerPrefab;

        public IPoolCreator<BugView> BugPoolCreator;
        public BugView BugPrefab;
    }
}