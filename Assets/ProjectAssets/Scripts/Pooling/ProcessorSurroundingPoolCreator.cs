using Project.Infrastructure;
using Project.UnityComponents;

namespace Project.Pooling
{
    public sealed class ProcessorSurroundingPoolCreator : PoolCreator<ProcessorSurroundingView>
    {
        protected override void Construct(SharedData data)
        {
            data.Assets.ProcessorSurroundingPoolCreator = this;
        }
    }
}