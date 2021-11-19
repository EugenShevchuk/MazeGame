using Project.Infrastructure;
using Project.UnityComponents;

namespace Project.Pooling
{
    public sealed class BugPoolCreator : PoolCreator<BugView>
    {
        protected override void Construct(SharedData data)
        {
            data.Assets.BugPoolCreator = this;
        }
    }
}