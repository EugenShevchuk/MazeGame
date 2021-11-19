using System;

namespace Project.Utilities
{
    public sealed class CountDownTimer : TimerBase
    {
        public event Action<float> TimerValueChangedEvent;
        public event Action TimerFinishedEvent;

        public CountDownTimer(TimerType type, float remainingTime)
        {
            Type = type;
            SetRemainingTime(remainingTime);
        }

        public void SetRemainingTime(float remainingTime)
        {
            Time = remainingTime;
            OnTimerValueChanged();
        }

        protected override void OnUpdateTick(float deltaTime)
        {
            base.OnUpdateTick(deltaTime);
            Time -= deltaTime;
            CheckFinish();
        }

        protected override void OnSecondTick()
        {
            base.OnSecondTick();
            Time -= 1f;
            CheckFinish();
        }

        private void CheckFinish()
        {
            if (Time <= 0)
                Stop();
            else
                OnTimerValueChanged();
        }

        protected override void OnTimerValueChanged()
        {
            TimerValueChangedEvent?.Invoke(Time);
        }

        protected override void OnTimerFinished()
        {
            TimerFinishedEvent?.Invoke();
        }
    }
}