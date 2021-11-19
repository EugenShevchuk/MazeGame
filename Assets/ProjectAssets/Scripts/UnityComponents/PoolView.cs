using Leopotam.EcsLite;
using Project.Interfaces;
using UnityEngine;

namespace Project.UnityComponents
{
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class PoolView : MonoBehaviour, IViewObject, IPoolObject
    {
        public GameObject GameObject => gameObject;
        public Transform Transform => transform;
        public EcsPackedEntityWithWorld Entity { get; set; }
        
        [SerializeField] private SpriteRenderer _renderer;

        public SpriteRenderer Renderer 
        {
            get
            {
                if (_renderer == null)
                    _renderer = GetComponent<SpriteRenderer>();

                return _renderer;
            }
        }

        public abstract void Recycle(bool checkDoubleRecycles = true);
    }
}
