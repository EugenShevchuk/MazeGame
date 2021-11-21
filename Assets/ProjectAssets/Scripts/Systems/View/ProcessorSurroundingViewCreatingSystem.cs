using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Infrastructure;
using Project.Interfaces;
using Project.Pooling;
using Project.UnityComponents;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class ProcessorSurroundingViewCreatingSystem : ViewCreatingSystem<Cell>
    {
        private Pool<ProcessorSurroundingView> _pool;

        public override void Init(EcsSystems systems)
        {
            Request = World.Filter<CreateViewRequest>()
                .Inc<Cell>()
                .Inc<ProcessorSurroundings>()
                .Exc<Processor>()
                .Exc<ObjectViewRef>()
                .End();
        }
        
        protected override IViewObject CreateView(ref Cell generic, Vector3 position)
        {
            _pool ??= Assets.ProcessorSurroundingPoolCreator.CreatePool(Assets.ProcessorSurroundingPrefab);

            var view = _pool.Get();
            
            view.transform.position = position;
            view.gameObject.SetActive(true);
            
            return view;
        }

        public ProcessorSurroundingViewCreatingSystem(EcsWorld world, SharedData data) : base(world, data)
        {
        }
    }
}