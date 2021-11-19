using System;
using Project.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Project.Pooling
{
    public sealed class Pool<T> where T : Object, IPoolObject
    {
        public T Prefab { get; }
        public bool IsExpandable { get; set; }
        public Transform Container { get; }

        private FastStack<T> _pool;

        public Pool(T prefab, int count)
        {
            Prefab = prefab;
            Container = null;
            
            CreatePool(count);
        }

        public Pool(T prefab, int count, Transform container)
        {
            Prefab = prefab;
            Container = container;
            
            CreatePool(count);
        }

        public T Get()
        {
            if (_pool.Count > 0)
                return _pool.Pop();
            
            if (IsExpandable)
                return CreateObject(true);

            throw new Exception($"There is no free elements in pool of type {typeof(T)}");
        }

        public void Recycle(IPoolObject poolObject, bool checkForDoubleRecycle = false)
        {
            if (poolObject != null)
            {
                
            }
        }

        private void CreatePool(int count)
        {
            _pool = new FastStack<T>(count);

            for (int i = 0; i < count; i++)
                CreateObject();
        }

        private T CreateObject(bool isActive = false)
        {
            var createdObject = Object.Instantiate(Prefab, Container);
            createdObject.GameObject.SetActive(isActive);
            _pool.Push(createdObject);
            return createdObject;
        }
    }
}