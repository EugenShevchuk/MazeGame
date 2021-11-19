using Leopotam.EcsLite;
using Reflex;
using Reflex.Scripts;
using UnityEngine;

namespace Project.Infrastructure
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private Configuration _configuration;
        [SerializeField] private AssetReferences _references;
        
        public override void InstallBindings(Container container)
        {
            container.BindSingleton(new EcsWorld());
            var data = new SharedData
            {
                Configuration = _configuration,
                References = _references
            };
            container.BindSingleton(data);
            container.BindSingleton<AssetReferences>(_references);
        }
    }
}