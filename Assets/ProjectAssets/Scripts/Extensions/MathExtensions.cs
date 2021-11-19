using Leopotam.Ecs.Types;
using Project.Components;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Extensions
{
    public static class MathExtensions
    {
        public static Vector2Int ToVector(this int2 int2)
        {
            return new Vector2Int(int2.x, int2.y);
        }
        
        
    }
}