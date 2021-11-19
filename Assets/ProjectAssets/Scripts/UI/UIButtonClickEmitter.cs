using Leopotam.EcsLite;
using Project.Extensions;
using Reflex.Scripts.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Project.UI
{
    public abstract class UIButtonClickEmitter<T> : MonoBehaviour where T : struct
    {
        [SerializeField] private Button _button;
        [MonoInject] public EcsWorld World;

        private void OnEnable() => _button.onClick.AddListener(OnClick);
        private void OnDisable() => _button.onClick.RemoveListener(OnClick);

        protected virtual void OnClick()
        {
            World.SendMessage(new T());
        }
    }
}