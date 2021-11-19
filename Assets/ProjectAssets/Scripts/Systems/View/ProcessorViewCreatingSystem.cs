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
    internal sealed class ProcessorViewCreatingSystem : ViewCreatingSystem<Cell>
    {
        private Pool<ProcessorView> _pool;
        
        public override void Init(EcsSystems systems)
        {
            Request = World.Filter<CreateViewRequest>()
                .Inc<Processor>()
                .Inc<Cell>()
                .Exc<ObjectViewRef>()
                .End();
        }

        protected override IViewObject CreateView(ref Cell generic, Vector3 position)
        {
            _pool ??= Assets.ProcessorPoolCreator.CreatePool(Assets.ProcessorPrefab);

            var view = _pool.Get();
            
            view.transform.position = position;
            view.gameObject.SetActive(true);
            
            return view;
        }
    }
}