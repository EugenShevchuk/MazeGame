using Project.Infrastructure;
using Project.UnityComponents;

namespace Project.Pooling
{
    public sealed class SpawnerPoolCreator : PoolCreator<SpawnerView>
    {
        protected override void Construct(SharedData data)
        {
            data.Assets.SpawnerPoolCreator = this;
        }
    }
}