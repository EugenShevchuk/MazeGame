using Project.Interfaces;
using UnityEngine;

namespace Project.Pooling
{
    public interface IPoolCreator<T> where T : Object, IPoolObject
    {
        public Pool<T> CreatePool(T poolObject);
    }
}