using Leopotam.EcsLite;
using Reflex.Scripts.Attributes;
using UnityEngine;

namespace Project.UI
{
    public sealed class EcsWorldProvider : MonoBehaviour
    {
        [SerializeField] private StartGameClickedEmitter _emitter;

        public void SetWorld(EcsWorld world)
        {
            _emitter.World = world;
        }
    }
}