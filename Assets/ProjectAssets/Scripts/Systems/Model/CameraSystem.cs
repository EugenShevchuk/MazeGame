using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Infrastructure;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class CameraSystem : IEcsRunSystem
    {
        [EcsShared] private readonly SharedData _data = default;

        [EcsFilter(typeof(CameraInput))]
        private readonly EcsFilter _movementInput = default;

        [EcsFilter(typeof(CameraComponent), typeof(WorldObject))]
        private readonly EcsFilter _camera = default;

        private readonly EcsPool<CameraInput> _movementInputPool = default;
        private readonly EcsPool<WorldObject> _worldObjectPool = default;
        private readonly EcsPool<CameraComponent> _cameraComponentPool = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _movementInput)
            {
                ref var cameraInput = ref _movementInputPool.Get(i);
                
                if (cameraInput.MouseInput != Vector2.zero)
                    MoveCamera(cameraInput.MouseInput);
                else if (cameraInput.KeyboardInput != Vector2.zero)
                    MoveCamera(cameraInput.KeyboardInput);
                
                if (cameraInput.ScrollWheelInput != 0)
                    ZoomScreen(cameraInput.ScrollWheelInput);
            }
        }

        private void MoveCamera(Vector2 direction)
        {
            foreach (var i in _camera)
            {
                var cameraTransform = _worldObjectPool.Get(i).Transform;
                var position = cameraTransform.position;
                position = Vector3.Lerp(position, position + (Vector3)direction, _data.Configuration.CameraMoveSpeed * Time.deltaTime);
                cameraTransform.position = position;
            }
        }

        private void ZoomScreen(float direction)
        {
            foreach (var i in _camera)
            {
                var camera = _cameraComponentPool.Get(i).Camera;
                var cameraSize = camera.orthographicSize;
                var config = _data.Configuration;
                var newCameraSize = Mathf.Clamp(cameraSize + direction, config.ZoomInMin, config.ZoomOutMax);
                camera.orthographicSize = Mathf.Lerp(cameraSize, newCameraSize, config.CameraZoomSpeed * Time.deltaTime);
            }
        }
    }
}