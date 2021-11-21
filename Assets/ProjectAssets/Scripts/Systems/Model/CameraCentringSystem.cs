using Leopotam.EcsLite;
using Project.Components;
using Project.Infrastructure;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class CameraCentringSystem : IEcsRunSystem
    {
        private readonly SharedData _data;

        private readonly EcsFilter _camera;

        private readonly EcsPool<CameraComponent> _cameraComponentPool;
        private readonly EcsPool<WorldObject> _worldObjectPool;

        public CameraCentringSystem(EcsWorld world, SharedData data)
        {
            _data = data;

            _camera = world.Filter<CameraComponent>().End();

            _cameraComponentPool = world.GetPool<CameraComponent>();
            _worldObjectPool = world.GetPool<WorldObject>();
        }

        public void Run(EcsSystems systems)
        {
            CenterCamera();
            SetCameraBounds();
            SetMaxCameraSize();
        }

        private void CenterCamera()
        {
            var center = (_data.CurrentLevel.MazeSize - 1) / 2;

            foreach (var i in _camera)
            {
                var cameraTransform = _worldObjectPool.Get(i).Transform;
                var camera = _cameraComponentPool.Get(i).Camera;
                
                cameraTransform.position = new Vector3(center, center, -10);
                camera.orthographicSize = _data.Configuration.MinCameraSize;
            }
        }

        private void SetCameraBounds()
        {
            var mazeSize = _data.CurrentLevel.MazeSize;

            foreach (var i in _camera)
            {
                ref var camData = ref _cameraComponentPool.Get(i);

                camData.MaxX = camData.MaxY = mazeSize + _data.Configuration.CameraBoundOffset - 1;
                camData.MinX = camData.MinY = 0 - _data.Configuration.CameraBoundOffset;
            }
        }

        private void SetMaxCameraSize()
        {
            var mazeWidth = (_data.CurrentLevel.MazeSize - 1) + _data.Configuration.CameraBoundOffset * 2;
            var maxOrthoSize = mazeWidth * Screen.height / Screen.width * .5f;

            _data.CurrentLevel.MaxCameraSize = maxOrthoSize;
        }
    }
}