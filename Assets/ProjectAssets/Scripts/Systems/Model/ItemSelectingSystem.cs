using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using Project.Extensions;
using Project.Interfaces;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class ItemSelectingSystem  : IEcsRunSystem
    {
        [EcsFilter(typeof(LeftMouseButtonPressedEvent))]
        private readonly EcsFilter _leftMouseButtonPressed = default;
        
        [EcsFilter(typeof(CameraComponent), typeof(WorldObject))]
        private readonly EcsFilter _camera = default;
        
        private readonly EcsPool<LeftMouseButtonPressedEvent> _leftMouseButtonPool = default;
        private readonly EcsPool<CameraComponent> _cameraComponentPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _leftMouseButtonPressed)
            {
                var mousePosition = _leftMouseButtonPool.Get(i).Position;
                
                foreach (var cam in _camera)
                {
                    var camera = _cameraComponentPool.Get(cam).Camera;
                    var ray = camera.ScreenPointToRay(mousePosition);

                    if (Physics.Raycast(ray, out var hit, 15))
                    {
                        if (hit.transform.TryGetComponent(out IViewObject view))
                            if (view.Entity.Unpack(out var world, out var entity))
                                entity.Add<Selected>(world);
                    }
                }
            }
        }
    }
}