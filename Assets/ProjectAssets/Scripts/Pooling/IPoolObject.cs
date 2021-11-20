using Project.Pooling;
using UnityEngine;

namespace Project.Interfaces
{
    public interface IPoolObject
    {
        public abstract GameObject GameObject { get; }
        public void Recycle(bool checkDoubleRecycles = true);
    }
}