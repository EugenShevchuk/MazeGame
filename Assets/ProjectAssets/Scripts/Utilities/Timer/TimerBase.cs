using System;

namespace Project.Utilities
{
    public abstract class TimerBase
    {
        public TimerType Type { get; protected set; }
        public bool IsPaused { get; protected set; }
        public float Time { get; protected set; }

        public virtual void Start()
        {
            IsPaused = false;
            Subscribe();
        }

        public virtual void Pause()
        {
            IsPaused = true;
            Unsubscribe();
            OnTimerValueChanged();
        }

        public virtual void Unpause()
        {
            IsPaused = false;
            Subscribe();
            OnTimerValueChanged();
        }

        public virtual void Stop()
        {
            IsPaused = true;
            Unsubscribe();
            OnTimerValueChanged();
            OnTimerFinished();
        }

        private void Subscribe()
        {
            switch (Type)
            {
                case TimerType.Tick:
                    TimeInvoker.Instance.UpdateTimeTickedEvent += OnUpdateTick;
                    break;
                case TimerType.UnscaledTick:
                    TimeInvoker.Instance.UpdateUnscaledTimeTickedEvent += OnUpdateTick;
                    break;
                case TimerType.Second:
                    TimeInvoker.Instance.SecondTickedEvent += OnSecondTick;
                    break;
                case TimerType.UnscaledSecond:
                    TimeInvoker.Instance.UnscaledSecondTickedEvent += OnSecondTick;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void Unsubscribe()
        {
            switch (Type)
            {
                case TimerType.Tick:
                    TimeInvoker.Instance.UpdateTimeTickedEvent -= OnUpdateTick;
                    break;
                case TimerType.UnscaledTick:
                    TimeInvoker.Instance.UpdateUnscaledTimeTickedEvent -= OnUpdateTick;
                    break;
                case TimerType.Second:
                    TimeInvoker.Instance.SecondTickedEvent -= OnSecondTick;
                    break;
                case TimerType.UnscaledSecond:
                    TimeInvoker.Instance.UnscaledSecondTickedEvent -= OnSecondTick;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected virtual void OnUpdateTick(float deltaTime)
        {
            if(IsPaused)
                return;
        }

        protected virtual void OnSecondTick()
        {
            if (IsPaused)
                return;
        }

        protected abstract void OnTimerValueChanged();

        protected abstract void OnTimerFinished();
    }
}