using System;
using UnityEngine;

namespace Project.Utilities
{
    public sealed class TimeInvoker : MonoBehaviour
    {
        public event Action<float> UpdateTimeTickedEvent;
        public event Action<float> UpdateUnscaledTimeTickedEvent;
        public event Action SecondTickedEvent;
        public event Action UnscaledSecondTickedEvent;

        private float _second;
        private float _unscaledSecond;
        
        private static TimeInvoker _instance;
        public static TimeInvoker Instance
        {
            get
            {
                if (_instance == null)
                {
                    var go = new GameObject("[TIME INVOKER]");
                    _instance = go.AddComponent<TimeInvoker>();
                    DontDestroyOnLoad(go);
                }

                return _instance;
            }
        }

        private void Update()
        {
            var deltaTime = Time.deltaTime;
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            
            UpdateTimeTickedEvent?.Invoke(deltaTime);
            UpdateUnscaledTimeTickedEvent?.Invoke(unscaledDeltaTime);

            _second += deltaTime;
            if (_second >= 1f)
            {
                _second -= 1f;
                SecondTickedEvent?.Invoke();
            }

            _unscaledSecond += unscaledDeltaTime;
            if (_unscaledSecond >= 1f)
            {
                _unscaledSecond -= 1f;
                UnscaledSecondTickedEvent?.Invoke();
            }
        }
    }
}