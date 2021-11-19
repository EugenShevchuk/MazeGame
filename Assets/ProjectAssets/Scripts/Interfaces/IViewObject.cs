using Leopotam.EcsLite;
using UnityEngine;

namespace Project.Interfaces
{
    public interface IViewObject
    {
        public Transform Transform { get; }
        public SpriteRenderer Renderer { get; }
        public EcsPackedEntityWithWorld Entity { get; set; }
    }
}