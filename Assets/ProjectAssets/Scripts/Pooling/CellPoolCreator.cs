using Project.Infrastructure;
using Project.UnityComponents;
using Reflex.Scripts.Attributes;

namespace Project.Pooling
{
    public sealed class CellPoolCreator : PoolCreator<CellView>
    {
        protected override void Construct(SharedData data)
        {
            data.Assets.CellPoolCreator = this;
        }
    }
}