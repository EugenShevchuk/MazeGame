using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Infrastructure;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class CameraMovementSystem : IEcsRunSystem
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
                var camera = _cameraComponentPool.Get(i);
                
                position = Vector3.Lerp(position, position + (Vector3)direction, _data.Configuration.CameraMoveSpeed * Time.deltaTime);
                
                cameraTransform.position = ClampCameraPosition(position, camera);
            }
        }

        private void ZoomScreen(float direction)
        {
            foreach (var i in _camera)
            {
                var camData = _cameraComponentPool.Get(i);
                var camTransform = _worldObjectPool.Get(i).Transform;
                var cameraSize = camData.Camera.orthographicSize;
                var config = _data.Configuration;
                
                var newCameraSize = Mathf.Clamp(cameraSize + direction, config.MinCameraSize, _data.CurrentLevel.MaxCameraSize);

                camData.Camera.orthographicSize = Mathf.Lerp(cameraSize, newCameraSize, config.CameraZoomSpeed * Time.deltaTime);
                camTransform.position = ClampCameraPosition(camTransform.position, camData);
            }
        }

        private Vector3 ClampCameraPosition(Vector3 target, in CameraComponent camData)
        {
            var height = camData.Camera.orthographicSize;
            var width = height * camData.Camera.aspect;

            
            var minX = camData.MinX + width;
            var maxX = camData.MaxX - width;
            var minY = camData.MinY + height;
            var maxY = camData.MaxY - height;

            var newX = Mathf.Clamp(target.x, minX, maxX);
            var newY = Mathf.Clamp(target.y, minY, maxY);

            return new Vector3(newX, newY, target.z);
        }
    }
}