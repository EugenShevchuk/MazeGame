using System.Collections.Generic;
using Leopotam.EcsLite;
using Project.Components;
using Project.Events;
using Project.Infrastructure;
using Project.Interfaces;
using Project.Pooling;
using Project.UnityComponents;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class CellViewCreatingSystem : ViewCreatingSystem<Cell>
    {
        private Pool<CellView> _pool;

        public CellViewCreatingSystem(EcsWorld world, SharedData data) : base(world, data)
        {
        }

        public override void Init(EcsSystems systems)
        {
            Request = World.Filter<CreateViewRequest>()
                .Inc<Cell>()
                .Exc<Processor>()
                .Exc<ProcessorSurroundings>()
                .Exc<ObjectViewRef>()
                .End();
        }

        protected override IViewObject CreateView(ref Cell cell, Vector3 position)
        {
            _pool ??= Assets.CellPoolCreator.CreatePool(Assets.CellPrefab);

            var view = _pool.Get();
            
            view.transform.position = position;
            view.gameObject.SetActive(true);
            
            return view;
        }
    }
}