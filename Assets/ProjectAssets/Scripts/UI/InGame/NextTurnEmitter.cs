using Project.Events;
using Project.Extensions;
using Project.Infrastructure;
using Reflex.Scripts.Attributes;

namespace Project.UI
{
    public sealed class NextTurnEmitter : UIButtonClickEmitter<PlayerEndedTurnEvent>
    {
        [MonoInject] private readonly SharedData _data = default;

        protected override void OnClick()
        {
            _data.CurrentWorld.SendMessage(new PlayerEndedTurnEvent());
        }
    }
}