using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;

namespace Project.Systems
{
    internal sealed class InputUpdateSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(CameraInput))]
        private readonly EcsFilter _cameraInput = default;

        private readonly EcsPool<CameraInput> _cameraInputPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _cameraInput)
            {
                ref var input = ref _cameraInputPool.Get(i);
            }
        }
    }
}