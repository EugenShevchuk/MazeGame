using System;
using UnityEngine;

namespace Project.Components
{
    [Serializable]
    public struct CameraComponent
    {
        public Camera Camera;
        [HideInInspector] public Vector2 Bounds;
        [HideInInspector] public float MinX;
        [HideInInspector] public float MaxX;
        [HideInInspector] public float MinY;
        [HideInInspector] public float MaxY;
    }
}