using System;
using Unity.Mathematics;
using UnityEngine;

namespace Project.Infrastructure
{
    [Serializable]
    public struct Level
    {
        public int MazeSize;
        [HideInInspector] public float MaxCameraSize;
        public int SpawnerCount;
        [Range(0, 1)] public float BraidChance;
        public int2 TurnsBetweenSpawns;
        public BugType BugTypes;
    }
}