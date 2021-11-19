using Leopotam.EcsLite;
using Project.Infrastructure;

namespace Project.Components
{
    public struct Bug
    {
        public BugType Type;
        public Cell CurrentCell;
        public EcsPackedEntityWithWorld ThisEntity;
        public EcsPackedEntityWithWorld Processor;
    }
}