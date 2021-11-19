using Project.Events;
using Project.Extensions;
using Project.Infrastructure;
using Project.Model.Algorithms;
using Reflex.Scripts.Attributes;

namespace Project.UI
{
    public sealed class WilsonClickEmitter : UIButtonClickEmitter<GenerateMazeRequest>
    {
        [MonoInject] private readonly SharedData _data = default;
        protected override void OnClick()
        {
            _data.CurrentWorld.CreateGenerateMazeRequest(new Wilson());
        }
    }
}