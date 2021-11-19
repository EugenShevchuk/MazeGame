using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Infrastructure;
using Project.Interfaces;
using Project.UnityComponents;
using UnityEngine;

namespace Project.Systems
{
    internal abstract class ViewCreatingSystem<T> : IEcsInitSystem, IEcsRunSystem where T : struct
    {
        protected readonly EcsWorld World = default;
        [EcsShared] protected readonly SharedData Data = default;
        protected AssetsData Assets => Data.Assets;
        
        protected EcsFilter Request;

        protected readonly EcsPool<CreateViewRequest> RequestPool = default;
        protected readonly EcsPool<WorldObject> WorldObjectPool = default;
        
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
                ref var request = ref RequestPool.Get(entity);
                ref var worldObject = ref WorldObjectPool.Add(entity);

                var view = CreateView(ref generic, request.SpawnPosition);
                view.Entity = World.PackEntityWithWorld(entity);
                
                ref var viewRef = ref World.GetPool<ObjectViewRef>().Add(entity);
                
                viewRef.View = view;
                worldObject.Transform = view.Transform;
                    
                RequestPool.Del(entity);
            }
        }

        protected abstract IViewObject CreateView(ref T generic, Vector3 position);
    }
}