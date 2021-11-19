using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Leopotam.EcsLite.UnityEditor;
using Project.Systems;
using Reflex.Scripts.Attributes;
using UnityEngine;

namespace Project.Infrastructure
{
    internal sealed class InGameUIStartup : MonoBehaviour
    {

        private EcsSystems _systems;
        
        private void Start()
        {
            /*
            _systems = new EcsSystems(_data.CurrentWorld, _data);

            _systems
#if UNITY_EDITOR
                .Add(new EcsWorldDebugSystem())
#endif
                
                
                .Inject()
                .Init();
                */
        }
       
        private void Update() 
        {
            _systems?.Run();
        }

        private void OnDestroy() 
        {
            if (_systems != null)
            {
                _systems.Destroy();
                _systems = null;
            }
        }
    }
}