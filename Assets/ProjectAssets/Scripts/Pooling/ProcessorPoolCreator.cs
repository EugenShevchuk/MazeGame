using Project.Infrastructure;
using Project.UnityComponents;
using Reflex.Scripts.Attributes;

namespace Project.Pooling
{
    public sealed class ProcessorPoolCreator : PoolCreator<ProcessorView>
    {
        protected override void Construct(SharedData data)
        {
            data.Assets.ProcessorPoolCreator = this;
        }
    }
}