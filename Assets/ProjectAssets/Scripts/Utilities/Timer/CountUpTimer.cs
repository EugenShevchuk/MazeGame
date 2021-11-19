using System;

namespace Project.Utilities
{
    public sealed class CountUpTimer : TimerBase
    {
        public event Action<float> TimerValueChangedEvent;
        public event Action<float> TimerFinishedEvent;

        public CountUpTimer(TimerType type)
        {
            Type = type;
        }

        protected override void OnUpdateTick(float deltaTime)
        {
            base.OnUpdateTick(deltaTime);
            Time += deltaTime;
            OnTimerValueChanged();
        }

        protected override void OnSecondTick()
        {
            base.OnSecondTick();
            Time += 1f;
            OnTimerValueChanged();
        }

        protected override void OnTimerValueChanged()
        {
            TimerValueChangedEvent?.Invoke(Time);
        }

        protected override void OnTimerFinished()
        {
            TimerFinishedEvent?.Invoke(Time);
        }
    }
}