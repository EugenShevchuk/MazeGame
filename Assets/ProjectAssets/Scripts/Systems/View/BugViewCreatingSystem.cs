using Project.Components;
using Project.Interfaces;
using Project.Pooling;
using Project.UnityComponents;
using UnityEngine;

namespace Project.Systems
{
    internal sealed class BugViewCreatingSystem : ViewCreatingSystem<Bug>
    {
        private Pool<BugView> _pool;
        
        protected override IViewObject CreateView(ref Bug generic, Vector3 position)
        {
            _pool ??= Assets.BugPoolCreator.CreatePool(Assets.BugPrefab);
            
            var view = _pool.Get();
            
            view.transform.position = position;
            view.gameObject.SetActive(true);
            
            return view;
        }
    }
}