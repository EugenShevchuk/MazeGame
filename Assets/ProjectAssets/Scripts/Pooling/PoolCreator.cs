using Project.Infrastructure;
using Project.Interfaces;
using Reflex.Scripts.Attributes;
using UnityEngine;

namespace Project.Pooling
{
    public abstract class PoolCreator<T> : MonoBehaviour, IPoolCreator<T> where T : MonoBehaviour, IPoolObject
    {
        [SerializeField] protected int _poolCount;
        [SerializeField] protected Transform _container;
        [SerializeField] protected bool _isExpandable;

        [MonoInject]
        protected abstract void Construct(SharedData data);
        
        public Pool<T> CreatePool(T poolObject)
        {
            return new Pool<T>(poolObject, _poolCount, _container)
            {
                IsExpandable = _isExpandable
            };
        }
    }
}