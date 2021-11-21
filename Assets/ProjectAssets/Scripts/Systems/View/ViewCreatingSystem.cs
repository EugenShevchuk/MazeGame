using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Infrastructure;
using Project.Interfaces;
using UnityEngine;

namespace Project.Systems
{
    internal abstract class ViewCreatingSystem<T> : IEcsInitSystem, IEcsRunSystem where T : struct
    {
        protected readonly EcsWorld World;
        protected readonly SharedData Data;
        
        protected AssetsData Assets => Data.Assets;
        protected EcsFilter Request;

        private readonly EcsPool<CreateViewRequest> _requestPool;
        private readonly EcsPool<WorldObject> _worldObjectPool;

        protected ViewCreatingSystem(EcsWorld world, SharedData data)
        {
            World = world;
            Data = data;
            
            _requestPool = world.GetPool<CreateViewRequest>();
            _worldObjectPool = world.GetPool<WorldObject>();
        }

        public virtual void Init(EcsSystems systems)
        {
            Request = World.Filter<CreateViewRequest>()
                .Inc<T>()
                .Exc<ObjectViewRef>()
                .End();
        }

        public virtual void Run(EcsSystems systems)
        {
            foreach (var entity in Request)
            {
                ref var generic = ref World.GetPool<T>().Get(entity);
                ref var request = ref _requestPool.Get(entity);
                ref var worldObject = ref _worldObjectPool.Add(entity);

                var view = CreateView(ref generic, request.SpawnPosition);
                view.Entity = World.PackEntityWithWorld(entity);
                
                ref var viewRef = ref World.GetPool<ObjectViewRef>().Add(entity);
                
                viewRef.View = view;
                worldObject.Transform = view.Transform;
                    
                _requestPool.Del(entity);
            }
        }

        protected abstract IViewObject CreateView(ref T generic, Vector3 position);
    }
}