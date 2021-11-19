using Project.Components;
using Project.Interfaces;
using Project.Pooling;
using Project.UnityComponents;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class SpawnerViewCreatingSystem : ViewCreatingSystem<Spawner>
    {
        private Pool<SpawnerView> _pool;
        
        protected override IViewObject CreateView(ref Spawner generic, Vector3 position)
        {
            _pool ??= Assets.SpawnerPoolCreator.CreatePool(Assets.SpawnerPrefab);
            
            var view = _pool.Get();
            
            view.transform.position = position;
            view.gameObject.SetActive(true);
            
            return view;
        }
    }
}