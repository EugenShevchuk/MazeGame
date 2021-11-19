using Project.Pooling;
using UnityEngine;

namespace Project.UnityComponents
{
    public sealed class CellView : PoolView
    {
        [SerializeField] private GameObject _leftWall;
        [SerializeField] private GameObject _rightWall;
        [SerializeField] private GameObject _bottomWall;
        [SerializeField] private GameObject _topWall;
        
        private Pool<CellView> _pool;
        public Pool<CellView> Pool
        {
            get => _pool;
            set => _pool = value;
        }

        public override void Recycle(bool checkDoubleRecycles = true)
        {
            _pool?.Recycle(this, checkDoubleRecycles);
        }

        public void SetupView(bool leftWall, bool rightWall, bool bottomWall, bool topWall)
        {
            _leftWall.SetActive(leftWall);
            _rightWall.SetActive(rightWall);
            _bottomWall.SetActive(bottomWall);
            _topWall.SetActive(topWall);
        }
    }
}