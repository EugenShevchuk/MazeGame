using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using Project.Events;

namespace Project.Systems
{
    internal sealed class TurnTransitionSystem : IEcsRunSystem
    {
        [EcsFilter(typeof(PlayerEndedTurnEvent))]
        private readonly EcsFilter _playerEndedTurn = default;
        
        public void Run(EcsSystems systems)
        {
            foreach (var i in _playerEndedTurn)
            {
                
            }
        }
    }
}