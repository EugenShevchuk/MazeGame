using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Project.UnityComponents
{
    public sealed class BugView : PoolView
    {
        public async UniTask Move(List<Vector3> path, float timeToMoveOneCell)
        {
            foreach (var point in path)
            {
                await Rotate(point);
                await transform.DOMove(point, timeToMoveOneCell).AsyncWaitForCompletion();
            }
        }

        private async UniTask Rotate(Vector3 nextPosition)
        {
            var direction = nextPosition - transform.position;
            
            if (direction == Vector3.up)
            {
                await transform.DORotate(Vector3.zero, 1f).AsyncWaitForCompletion();
            }

            else if (direction == Vector3.down)
            {
                await transform.DORotate(new Vector3(0, 0, 180), 1f).AsyncWaitForCompletion();
            }

            else if (direction == Vector3.left)
            {
                await transform.DORotate(new Vector3(0, 0, 90), 1f).AsyncWaitForCompletion();
            }

            else if (direction == Vector3.right)
            {
                await transform.DORotate(new Vector3(0, 0, -90), 1f).AsyncWaitForCompletion();
            }
        }

        public override void Recycle(bool checkDoubleRecycles = true)
        {
            
        }
    }
}