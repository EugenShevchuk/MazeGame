using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Components;
using Project.Events;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class InputSystem : IEcsInitSystem, IEcsRunSystem, IEcsDestroySystem
    {
        private readonly EcsWorld _world = default;

        [EcsFilter(typeof(MousePosition), typeof(CameraInput))] 
        private readonly EcsFilter _mouseControls = default;

        private readonly EcsPool<MousePosition> _mousePositionPool = default;
        private readonly EcsPool<LeftMouseButtonPressedEvent> _leftMouseEventPool = default;
        private readonly EcsPool<CameraInput> _cameraInputPool = default;
        
        private Actions _actions;
        
        public void Init(EcsSystems systems)
        {
            _actions = new Actions();
            _actions.Enable();

            var entity = _world.NewEntity();
            _mousePositionPool.Add(entity);
            _cameraInputPool.Add(entity);
            
            _actions.Mouse.LeftMouseButton.performed += _ => OnLeftMouseButtonClicked();
            _actions.Keyboard.Movement.performed += _ => OnKeyboardMoveInput();
            _actions.Keyboard.Movement.canceled += _ => OnKeyboardMoveInputCanceled();

            _actions.Mouse.Zoom.performed += _ => OnMouseWheelScrolled();
            _actions.Mouse.Zoom.canceled += _ => OnMouseWheelScrollCanceled();
        }

        public void Destroy(EcsSystems systems)
        {
            _actions.Disable();
            _actions.Dispose();
        }

        public void Run(EcsSystems systems)
        {
            foreach (var i in _mouseControls)
            {
                ref var mouse = ref _mousePositionPool.Get(i);
                mouse.Position = _actions.Mouse.MousePosition.ReadValue<Vector2>();

                ref var cameraInput = ref _cameraInputPool.Get(i);

                if (mouse.Position.y >= Screen.height * .98f)
                    cameraInput.MouseInput.y = 1;
                else if (mouse.Position.y <= Screen.height * .02f)
                    cameraInput.MouseInput.y = -1;
                
                if (mouse.Position.x >= Screen.width * .98f)
                    cameraInput.MouseInput.x = 1;
                else if (mouse.Position.x <= Screen.width * .02f)
                    cameraInput.MouseInput.x = -1;

                if (mouse.Position.x >= Screen.width * .025f && mouse.Position.x <= Screen.width * .975f &&
                    mouse.Position.y >= Screen.height * .025f && mouse.Position.y <= Screen.height * .975f)
                {
                    cameraInput.MouseInput = Vector2.zero;
                }
            }
        }

        private void OnLeftMouseButtonClicked()
        {
            var i = _world.NewEntity();
            ref var leftMouseEvent = ref _leftMouseEventPool.Add(i);
            leftMouseEvent.Position = _actions.Mouse.MousePosition.ReadValue<Vector2>();
        }

        private void OnKeyboardMoveInput()
        {
            foreach (var i in _mouseControls)
            {
                ref var cameraInput = ref _cameraInputPool.Get(i);
                cameraInput.KeyboardInput = _actions.Keyboard.Movement.ReadValue<Vector2>();
            }
        }

        private void OnKeyboardMoveInputCanceled()
        {
            foreach (var i in _mouseControls)
            {
                ref var cameraInput = ref _cameraInputPool.Get(i);
                cameraInput.KeyboardInput = Vector2.zero;
            }
        }

        private void OnMouseWheelScrolled()
        {
            foreach (var i in _mouseControls)
            {
                ref var cameraInput = ref _cameraInputPool.Get(i);
                cameraInput.ScrollWheelInput = _actions.Mouse.Zoom.ReadValue<float>();
            }
        }

        private void OnMouseWheelScrollCanceled()
        {
            foreach (var i in _mouseControls)
            {
                ref var cameraInput = ref _cameraInputPool.Get(i);
                cameraInput.ScrollWheelInput = 0;
            }
        }
    }
}